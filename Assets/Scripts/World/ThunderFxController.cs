using UnityEngine;

public class ThunderFxController : MonoBehaviour
{
    [SerializeField] private Transform check;
    [SerializeField] private Vector2 boxSize;
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private CharacterStats myStats;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Vector2 surroundingCheckSize;

    [SerializeField] private AudioSource audioSource;

    private bool hasDamaged = false;

    private void AnimationTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(check.position, boxSize, whatIsPlayer);

        foreach (var hit in colliders)
        {
            if (hasDamaged) continue;

            if (hit.GetComponent<Player>() != null || hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Entity>().SetupKnockbackDir(transform);
                myStats.DoDamage(hit.GetComponent<CharacterStats>());
                hasDamaged = true;
            }
        }
    }

    private void PlayThunderSFX()
    {
        audioSource.Play();
    }

    public void FindPosition()
    {
        Vector3 newPosition = new Vector3(transform.position.x, transform.position.y - GroundBelow().distance + (boxSize.y / 2) - 0.35f);

        transform.position = newPosition;
    }

    private RaycastHit2D GroundBelow() => Physics2D.Raycast(transform.position, Vector2.down, 100, whatIsGround);
    private bool SomethingIsAround() => Physics2D.BoxCast(transform.position, surroundingCheckSize, 0, Vector2.zero, 0, whatIsGround);

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(check.position, boxSize);
        Gizmos.DrawWireCube(transform.position, surroundingCheckSize);
    }

    private void DestroySelf() => Destroy(gameObject);

    public void DisableSelf() => gameObject.SetActive(false);
}
