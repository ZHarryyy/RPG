using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AltarOfThunderClawController : MonoBehaviour
{   
    private bool canActivate;
    private bool isActivate;
    private CanvasGroup canvasGroup;

    private bool isLoading = false;
    public string sceneName;

    private void Start()
    {
        canvasGroup = GetComponentInChildren<CanvasGroup>();
    }

    private void Update()
    {
        if (canActivate && Input.GetKeyDown(KeyCode.E) && !isActivate && !isLoading) ActivateThunderClaw();
    }

    private void ActivateThunderClaw()
    {
        isActivate = true;
        StartCoroutine(LoadSceneAsync());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            canActivate = true;

            if (!isActivate)
            {
                StartCoroutine(FadeInKeyPrompt());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            canActivate = false;

            StartCoroutine(FadeOutKeyPrompt());
        }
    }

    private IEnumerator FadeInKeyPrompt()
    {
        float startAlpha = canvasGroup.alpha;
        float targetAlpha = 1f;
        float duration = 1f;

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            float currentAlpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            canvasGroup.alpha = currentAlpha;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator FadeOutKeyPrompt()
    {
        float startAlpha = canvasGroup.alpha;
        float targetAlpha = 0f;
        float duration = 1f;

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            float currentAlpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            canvasGroup.alpha = currentAlpha;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator LoadSceneAsync()
    {
        isLoading = true;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);

            if (progress >= 0.9f)
            {
                yield return new WaitForSeconds(3f);
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
