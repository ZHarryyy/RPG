using System.Collections;
using UnityEngine;

public class LostSoulController : MonoBehaviour
{
    public int currency;
    private bool canCollect;
    [SerializeField] private Transform targetUI;
    private Animator animator;
    private bool isAnimationPlaying;

    private void Start()
    {
        animator = GetComponent<Animator>();

        isAnimationPlaying = true;
        targetUI = GameObject.FindGameObjectWithTag("SoulIcon").transform;
    }

    private void Update()
    {
        if (canCollect && Input.GetKeyDown(KeyCode.E))
        {
            isAnimationPlaying = true;
            StartCoroutine(PlayAndMove());
        }

        if (isAnimationPlaying)
        {
            animator.speed = 1f;
        }
        else animator.speed = 0f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null) canCollect = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null) canCollect = false;
    }

    private IEnumerator PlayAndMove()
    {
        animator.SetTrigger("isCollect");

        yield return new WaitForSeconds(0.3f);

        isAnimationPlaying = false;

        Vector3 startPos = transform.position;

        Vector3 targetPos = GetTargetWorldPosition();

        float firstElapsedTime = 0f;
        float firstDuration = 0.8f;

        while (firstElapsedTime < firstDuration)
        {
            targetPos = GetTargetWorldPosition();
            targetPos.z = startPos.z;

            transform.position = Vector3.Lerp(startPos, targetPos, firstElapsedTime / firstDuration);

            firstElapsedTime += Time.deltaTime;
            yield return null;
        }

        isAnimationPlaying = true;

        float lastElapsedTime = 0f;
        float lastDuration = 0.4f;

        while (lastElapsedTime < lastDuration)
        {
            targetPos = GetTargetWorldPosition();
            targetPos.z = startPos.z;

            transform.position = Vector3.Lerp(transform.position, targetPos, lastElapsedTime / lastDuration);

            lastElapsedTime += Time.deltaTime;
            yield return null;
        }

        PlayerManager.instance.currency += currency;
        Destroy(this.gameObject);
    }

    private Vector3 GetTargetWorldPosition()
    {
        Vector3 targetWorldPos = Camera.main.ScreenToWorldPoint(targetUI.position);
        return targetWorldPos;
    }
}
