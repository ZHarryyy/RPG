using System.Collections;
using UnityEngine;

public class UI_Instruction : MonoBehaviour
{
    [SerializeField] private GameObject HealInstruction;
    [SerializeField] private GameObject ParryInstruction;

    private CanvasGroup canvasGroup;

    private bool healIsActivate;
    private bool parryIsActivate;

    private float duration = 10f;

    private void Update()
    {
        if (healIsActivate || parryIsActivate) duration -= Time.unscaledDeltaTime;

        if ((Input.GetKeyDown(KeyCode.H) || duration <= 0) && healIsActivate)
        {
            StartCoroutine(FadeOutCanvasGroup());
            StartCoroutine(WaitForParryInstruction());
        }

        if ((Input.GetKeyDown(KeyCode.Q) || duration <= 0) && parryIsActivate)
        {
            StartCoroutine(FadeOutCanvasGroup());
            parryIsActivate = false;
        }
    }

    public IEnumerator activateInstruction()
    {
        yield return new WaitForSeconds(3f);

        HealInstruction.SetActive(true);
        healIsActivate = true;

        canvasGroup = HealInstruction.GetComponent<CanvasGroup>();

        StartCoroutine(FadeInCanvasGroup());
    }


    private IEnumerator FadeInCanvasGroup()
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
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }
    }

    private IEnumerator FadeOutCanvasGroup()
    {
        float fadeDuration = 1f; // 渐隐持续时间
        float targetAlpha = 0f;
        float startAlpha = canvasGroup.alpha;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            canvasGroup.alpha = alpha;
            yield return null;
        }

        canvasGroup.alpha = targetAlpha;
    }

    private IEnumerator WaitForParryInstruction()
    {
        healIsActivate = false;
        yield return new WaitForSeconds(10f);

        ParryInstruction.SetActive(true);
        duration = 10f;
        parryIsActivate = true;

        canvasGroup = ParryInstruction.GetComponent<CanvasGroup>();

        StartCoroutine(FadeInCanvasGroup());
    }
}
