using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class AltarOfThunderClawController : MonoBehaviour
{
    [SerializeField] private Player player;

    private bool canActivate;
    private bool isActivate;
    private CanvasGroup canvasGroup;
    [SerializeField] private PlayableDirector timeline;

    private bool isLoading = false;
    public string sceneName;

    private void Start()
    {
        canvasGroup = GetComponentInChildren<CanvasGroup>();
    }

    private void Update()
    {
        if (canActivate && Input.GetKeyDown(KeyCode.E) && !isActivate && !isLoading)
        {
            StartCoroutine(MovePlayerToPosition(player.transform, transform.position));
            StartCoroutine(FadeOutKeyPrompt());
        }
    }

    private IEnumerator MovePlayerToPosition(Transform playerTransform, Vector3 targetPosition)
    {
        isActivate = true;

        float direction = Mathf.Sign(targetPosition.x - playerTransform.position.x);
        player.stateMachine.ChangeState(player.moveState);
        player.GetComponent<Player>().enabled = false;
        StartCoroutine(player.BusyFor(Mathf.Abs(playerTransform.position.x - targetPosition.x) / player.moveSpeed));
        player.SetVelocity(player.moveSpeed * direction, player.rb.velocity.y);

        while (Mathf.Abs(playerTransform.position.x - targetPosition.x) > 0.1f)
        {
            yield return null;
        }

        player.SetVelocity(0f, player.rb.velocity.y);
        player.stateMachine.ChangeState(player.idleState);

        ActivateThunderClaw();
        StartCoroutine(player.BusyFor((float)timeline.duration));
    }

    private void ActivateThunderClaw()
    {
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

        timeline.Play();

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);

            if (progress >= 0.9f)
            {
                yield return new WaitForSeconds((float)timeline.duration);
                SaveManager.instance.DeleteSaveData();
                AudioManager.instance.StopAllBGM();
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
