using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_MainMenu : MonoBehaviour
{
    private string sceneName;
    [SerializeField] private GameObject continueButton;
    [SerializeField] private UI_FadeScreen fadeScreen;
    [SerializeField] private PlayableDirector timeline;

    [SerializeField] private List<Button> buttons;
    private Dictionary<Button, CanvasGroup> buttonCanvasGroups = new Dictionary<Button, CanvasGroup>();
    private Dictionary<CanvasGroup, Coroutine> fadeCoroutines = new Dictionary<CanvasGroup, Coroutine>();
    private float fadeDuration = 0.5f;

    private void Start()
    {
        if (SaveManager.instance.HasSavedData() == false)
        {
            continueButton.SetActive(false);
        }

        foreach (var button in buttons)
        {
            // 获取按钮下的子对象的 CanvasGroup 组件
            CanvasGroup canvasGroup = button.GetComponentInChildren<CanvasGroup>();

            // 初始化子对象的透明度为0
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 0f;
            }

            // 为每个按钮对象添加 EventTrigger 组件
            EventTrigger eventTrigger = button.GetComponent<EventTrigger>();

            if (eventTrigger == null)
            {
                eventTrigger = button.gameObject.AddComponent<EventTrigger>();
            }

            // 创建进入按钮范围的事件条目
            EventTrigger.Entry pointerEnterEntry = new EventTrigger.Entry();
            pointerEnterEntry.eventID = EventTriggerType.PointerEnter;
            pointerEnterEntry.callback.AddListener((data) => { OnMouseEnterButton(button); });
            eventTrigger.triggers.Add(pointerEnterEntry);

            // 创建离开按钮范围的事件条目
            EventTrigger.Entry pointerExitEntry = new EventTrigger.Entry();
            pointerExitEntry.eventID = EventTriggerType.PointerExit;
            pointerExitEntry.callback.AddListener((data) => { OnMouseExitButton(button); });
            eventTrigger.triggers.Add(pointerExitEntry);

            // 添加按钮和其子对象的 CanvasGroup 到字典中
            buttonCanvasGroups.Add(button, canvasGroup);
        }
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
        StartCoroutine(LoadSceneWithTimeline());
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    IEnumerator LoadSceneWithFadeEffect()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        fadeScreen.FadeOut();

        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
            {
                yield return new WaitForSeconds(1.5f);
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    IEnumerator LoadSceneWithTimeline()
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
    public void OnMouseEnterButton(Button button)
    {
        // 获取按钮下的子对象的 CanvasGroup 组件
        CanvasGroup canvasGroup = buttonCanvasGroups[button];

        // 开始渐显效果
        if (canvasGroup != null)
        {
            // 取消之前正在进行的渐隐效果
            if (fadeCoroutines.ContainsKey(canvasGroup))
            {
                Coroutine fadeCoroutine = fadeCoroutines[canvasGroup];
                StopCoroutine(fadeCoroutine);
                fadeCoroutines.Remove(canvasGroup);
            }

            Coroutine newFadeCoroutine = StartCoroutine(FadeCanvasGroup(canvasGroup, 1f));
            fadeCoroutines.Add(canvasGroup, newFadeCoroutine);
        }
    }

    public void OnMouseExitButton(Button button)
    {
        // 获取按钮下的子对象的 CanvasGroup 组件
        CanvasGroup canvasGroup = buttonCanvasGroups[button];

        // 开始渐隐效果
        if (canvasGroup != null)
        {
            // 取消之前正在进行的渐显效果
            if (fadeCoroutines.ContainsKey(canvasGroup))
            {
                Coroutine fadeCoroutine = fadeCoroutines[canvasGroup];
                StopCoroutine(fadeCoroutine);
                fadeCoroutines.Remove(canvasGroup);
            }

            Coroutine newFadeCoroutine = StartCoroutine(FadeCanvasGroup(canvasGroup, 0f));
            fadeCoroutines.Add(canvasGroup, newFadeCoroutine);
        }
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float targetAlpha)
    {
        float startAlpha = canvasGroup.alpha;

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            float t = elapsedTime / fadeDuration;

            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = targetAlpha;

        fadeCoroutines.Remove(canvasGroup);
    }
}
