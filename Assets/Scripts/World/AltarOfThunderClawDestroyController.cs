using System.Collections;
using UnityEngine;

public class AltarOfThunderClawDestroyController : MonoBehaviour
{
    [SerializeField] private Player player;

    private bool canActivate;
    public bool isActivate;
    private CanvasGroup canvasGroup;
    [SerializeField] private GameObject[] Arrows;
    [SerializeField] private UI_InGame uI_HealthBar;
    [SerializeField] private UI_Instruction uI_Instruction;

    private void Start()
    {
        canvasGroup = GetComponentInChildren<CanvasGroup>();
    }

    private void Update()
    {
        if (!isActivate) player.anim.speed = 0;

        if (Input.GetKeyDown(KeyCode.B) && canActivate && !isActivate)
        {
            isActivate = true;

            player.stats.currentHealth = player.stats.maxHealth.GetValue() / 2;
            uI_HealthBar.UpdateHealthUI();

            StopCoroutine(FadeInAndOutCanvasGroup());
            StartCoroutine(activateArrows());
            StartCoroutine(FadeOutCanvasGroup());
            StartCoroutine(uI_Instruction.activateInstruction());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            canActivate = true;

            if (!isActivate)
            {
                StartCoroutine(FadeInAndOutCanvasGroup());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            canActivate = false;
            StopCoroutine(FadeInAndOutCanvasGroup());
            canvasGroup.alpha = 0f;
        }
    }

    private IEnumerator FadeInAndOutCanvasGroup()
    {
        while (canActivate && !isActivate)
        {
            float targetAlpha = canvasGroup.alpha > 0f ? 0f : 1f;
            float currentAlpha = canvasGroup.alpha;
            float alphaSpeed = 1.5f; // 控制闪烁的速度

            while (!Mathf.Approximately(currentAlpha, targetAlpha))
            {
                currentAlpha = Mathf.MoveTowards(currentAlpha, targetAlpha, alphaSpeed * Time.deltaTime);
                canvasGroup.alpha = currentAlpha;
                yield return null;
            }

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
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            canvasGroup.alpha = alpha;
            yield return null;
        }

        canvasGroup.alpha = targetAlpha;
    }

    private IEnumerator activateArrows()
    {
        for (int i = 0; i < Arrows.Length; i++)
        {
            Arrows[i].SetActive(true);
        }

        // 获取箭头初始缩放
        Vector3 initialScale = Arrows[0].transform.localScale;

        // 定义缩放动画的参数
        float scaleDuration = 0.5f; // 缩放动画持续时间
        float targetScale = 1.8f; // 目标缩放大小
        float elapsedTime = 0f;

        // 执行缩放动画
        while (elapsedTime < scaleDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;

            // 计算当前缩放比例
            float scale = Mathf.Lerp(initialScale.x, targetScale, elapsedTime / scaleDuration);

            // 缩放所有箭头
            for (int i = 0; i < Arrows.Length; i++)
            {
                Arrows[i].transform.localScale = new Vector3(scale, scale, scale);
            }

            yield return null;
        }


        yield return new WaitForSecondsRealtime(3f);

        for (int i = 0; i < Arrows.Length; i++)
        {
            Arrows[i].SetActive(false);
            Arrows[i].transform.localScale = initialScale;
        }
    }
}
