using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private float sfxMinimumDistance;
    [SerializeField] public AudioSource[] sfx;
    [SerializeField] private AudioSource[] bgm;

    [SerializeField] private Dictionary<string, int> bgmIndexByScene;

    public bool playBgm;
    private int bgmIndex;

    private void Awake()
    {
        if (instance != null) Destroy(instance.gameObject);
        else instance = this;

        bgmIndexByScene = new Dictionary<string, int>();
        bgmIndexByScene.Add("Level0", 0);
        bgmIndexByScene.Add("LevelArena", 6);
    }

    private void Start()
    {
        PlayBGMByScene(SceneManager.GetActiveScene().name);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayBGMByScene(scene.name);
    }

    private void Update()
    {
        if (!playBgm)
        {
            StopAllBGM();
        }
    }

    public void PlaySFX(int _sfxIndex, Transform _source)
    {
        // if (sfx[_sfxIndex].isPlaying) return;

        if (_source != null && Vector2.Distance(PlayerManager.instance.player.transform.position, _source.position) > sfxMinimumDistance) return;

        if (_sfxIndex < sfx.Length)
        {
            sfx[_sfxIndex].pitch = Random.Range(.85f, 1.1f);
            sfx[_sfxIndex].Play();
        }
    }

    public void StopSFX(int _index) => sfx[_index].Stop();

    public IEnumerator IncreaseVolumeOverTime(int _sfxIndex, float currentVolume, float fadeTime)
    {
        float startTime = Time.time;
        float elapsedTime = 0f;
        float startVolume = currentVolume;

        while (elapsedTime < fadeTime)
        {
            elapsedTime = Time.time - startTime;
            float normalizedTime = elapsedTime / fadeTime;
            float targetVolume = Mathf.Lerp(startVolume, 1f, normalizedTime);
            sfx[_sfxIndex].volume = targetVolume;
            yield return null;
        }

        sfx[_sfxIndex].volume = 1f;
    }

    public IEnumerator DecreaseVolumeOverTime(int _sfxIndex, float currentVolume, float fadeTime)
    {
        float startTime = Time.time;
        float elapsedTime = 0f;
        float startVolume = currentVolume;

        while (elapsedTime < fadeTime)
        {
            elapsedTime = Time.time - startTime;
            float normalizedTime = elapsedTime / fadeTime;
            float targetVolume = Mathf.Lerp(startVolume, 0f, normalizedTime);
            sfx[_sfxIndex].volume = targetVolume;
            yield return null;
        }

        sfx[_sfxIndex].Stop();
        sfx[_sfxIndex].volume = startVolume;
    }

    public void PlayRandomBGM()
    {
        bgmIndex = Random.Range(0, bgm.Length);
        PlayBGM(bgmIndex);
    }

    public void PlayBGM(int _bgmIndex)
    {
        bgmIndex = _bgmIndex;

        StopAllBGM();
        bgm[bgmIndex].Play();
    }

    public void StopAllBGM()
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }
    }

    private void PlayBGMByScene(string sceneName)
    {
        if (bgmIndexByScene.ContainsKey(sceneName))
        {
            int bgmIndex = bgmIndexByScene[sceneName];
            PlayBGM(bgmIndex);
        }
        else
        {
            // 如果没有为当前场景指定 BGM 索引，则停止所有 BGM
            StopAllBGM();
        }
    }
}
