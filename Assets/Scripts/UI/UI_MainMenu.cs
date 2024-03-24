using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class UI_MainMenu : MonoBehaviour
{
    private string sceneName;
    [SerializeField] private GameObject continueButton;
    [SerializeField] private UI_FadeScreen fadeScreen;
    [SerializeField] private PlayableDirector timeline;

    private void Start()
    {
        if (SaveManager.instance.HasSavedData() == false) continueButton.SetActive(false);
    }

    public void Continue()
    {
        if (GameManager.instance.currentSceneName != null) sceneName = GameManager.instance.currentSceneName;
        StartCoroutine(LoadSceneWithFadeEffect());
    }

    public void NewGame()
    {
        SaveManager.instance.DeleteSaveData();
        sceneName = "Level0";
        AudioManager.instance.StopAllBGM();
        fadeScreen.FadeOut();
        StartCoroutine(LoadSceneWithFadeEffect());
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    IEnumerator LoadSceneWithFadeEffect()
    {       
        timeline.Play();

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
            {
                yield return new WaitForSeconds((float)timeline.duration);
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
