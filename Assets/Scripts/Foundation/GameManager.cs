using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, ISaveManager
{
    public static GameManager instance;
    private Transform player;
    [SerializeField] private Checkpoint[] checkpoints;
    [SerializeField] private string closestCheckpointId;
    public string currentSceneName;
    [SerializeField] private PlayableDirector timeline;

    [Header("Lost currency")]
    [SerializeField] private GameObject lostSoulPrefab;
    public int lostSoulAmount;
    [SerializeField] private float lostSoulX;
    [SerializeField] private float lostSoulY;

    private void Awake()
    {
        if (instance != null) Destroy(instance.gameObject);
        else instance = this;

        currentSceneName = SceneManager.GetActiveScene().name;
    }

    private void Start()
    {
        checkpoints = FindObjectsOfType<Checkpoint>();
        if (PlayerManager.instance != null) player = PlayerManager.instance.player.transform;
    }

    public void RestartScene()
    {
        SaveManager.instance.SaveGame();
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void LoadData(GameData _data) => StartCoroutine(LoadWithDelay(_data));

    private void LoadCheckpoints(GameData _data)
    {
        foreach (KeyValuePair<string, bool> pair in _data.checkpoints)
        {
            foreach (Checkpoint checkpoint in checkpoints)
            {
                if (checkpoint.Id == pair.Key && pair.Value == true) checkpoint.ActivateCheckpoint();
            }
        }
    }

    private void LoadLostCurrency(GameData _data)
    {
        lostSoulAmount = _data.lostSoulAmount;
        lostSoulX = _data.lostSoulX;
        lostSoulY = _data.lostSoulY;

        if (lostSoulAmount > 0)
        {
            GameObject newLostSoul = Instantiate(lostSoulPrefab, new Vector3(lostSoulX, lostSoulY), Quaternion.identity);
            newLostSoul.GetComponent<LostSoulController>().currency = lostSoulAmount;
        }

        lostSoulAmount = 0;
    }

    private void LoadScene(GameData _data)
    {
        currentSceneName = _data.currentSceneName;
    }

    private IEnumerator LoadWithDelay(GameData _data)
    {
        yield return new WaitForSeconds(.1f);

        LoadScene(_data);
        LoadCheckpoints(_data);
        LoadClosestCheckpoint(_data);
        LoadLostCurrency(_data);
    }

    public void SaveData(ref GameData _data)
    {
        _data.lostSoulAmount = lostSoulAmount;
        _data.lostSoulX = player.position.x;
        _data.lostSoulY = player.position.y;

        if (FindClosestCheckpoint() != null) _data.closestCheckpointId = FindClosestCheckpoint().Id;

        _data.checkpoints.Clear();

        foreach (Checkpoint checkpoint in checkpoints)
        {
            _data.checkpoints.Add(checkpoint.Id, checkpoint.activationStatus);
        }

        _data.currentSceneName = SceneManager.GetActiveScene().name;
    }


    private void LoadClosestCheckpoint(GameData _data)
    {
        if (_data.closestCheckpointId == null) return;

        closestCheckpointId = _data.closestCheckpointId;

        foreach (Checkpoint checkpoint in checkpoints)
        {
            if (closestCheckpointId == checkpoint.Id) player.position = checkpoint.transform.position;
        }
    }

    private Checkpoint FindClosestCheckpoint()
    {
        float closestDistance = Mathf.Infinity;
        Checkpoint closestCheckpoint = null;

        foreach (var checkpoint in checkpoints)
        {
            float distanceToCheckpoint = Vector2.Distance(player.position, checkpoint.transform.position);

            if (distanceToCheckpoint < closestDistance && checkpoint.activationStatus == true)
            {
                closestDistance = distanceToCheckpoint;
                closestCheckpoint = checkpoint;
            }
        }

        return closestCheckpoint;
    }

    public void PauseGame(bool _pause)
    {
        if (_pause) Time.timeScale = 0;
        else Time.timeScale = 1;
    }

    public void LoadNextSceneAfterTimeline()
    {
        StartCoroutine(LoadSceneWithFadeEffect("Level1"));
    }

    private IEnumerator LoadSceneWithFadeEffect(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
            {
                yield return new WaitForSeconds(7.7f);
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
