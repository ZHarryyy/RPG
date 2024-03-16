using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Bonfire : MonoBehaviour
{
    private bool canLitFire;
    private bool isLited;

    private Animator anim;
    private Light2D light2D;
    private CanvasGroup canvasGroup;

    private void Start()
    {
        anim = GetComponent<Animator>();
        light2D = GetComponentInChildren<Light2D>();
        canvasGroup = GetComponentInChildren<CanvasGroup>();
    }

    private void Update()
    {
        if (canLitFire && Input.GetKeyDown(KeyCode.E) && !isLited) LitBonfire();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            canLitFire = true;

            if (!isLited)
            {
                StartCoroutine(FadeInKeyPrompt());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            canLitFire = false;

            StartCoroutine(FadeOutKeyPrompt());
        }
    }

    public void LitBonfire()
    {
        AudioManager.instance.PlaySFX(4, transform);
        isLited = true;
        anim.SetTrigger("bonfire");
        light2D.enabled = true;
        StartCoroutine(FadeOutKeyPrompt());
        GameObject.Find("Canvas").GetComponent<Level0UI>().SwitchOnBonfireLitScreen();
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
