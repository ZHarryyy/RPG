using System.Collections;
using UnityEngine;

public class Instruction : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    private void Start()
    {
        canvasGroup = GetComponentInChildren<CanvasGroup>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null) StartCoroutine(FadeInKeyPrompt());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null) StartCoroutine(FadeOutKeyPrompt());
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
}
