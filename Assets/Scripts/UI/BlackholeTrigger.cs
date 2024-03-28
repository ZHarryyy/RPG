using System.Collections;
using UnityEngine;

public class BlackholeTrigger : MonoBehaviour
{
    [SerializeField] private UI_InGame ui_InGame;

    private CanvasGroup canvasGroup;
    private bool isActivate;

    private void Start()
    {
        canvasGroup = GetComponentInChildren<CanvasGroup>();
    }

    private void Update()
    {
        if (!PlayerManager.instance.player.isDieFirstTime)
        {
            StartCoroutine(FadeInAndOutCanvasGroup());
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ui_InGame.UpdateHealthUI();
            this.gameObject.SetActive(false);
        }
    }

    private IEnumerator FadeInAndOutCanvasGroup()
    {
        while (!isActivate)
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

        isActivate = true;
    }
}
