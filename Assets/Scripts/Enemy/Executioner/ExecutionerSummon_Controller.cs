using UnityEngine;

public class ExecutionerSummon_Controller : MonoBehaviour
{
    [SerializeField] private Transform check;
    [SerializeField] private Vector2 boxSize;
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private LayerMask whatIsGround;
    private Animator anim => GetComponent<Animator>();
    private Rigidbody2D rb => GetComponent<Rigidbody2D>();

    private CharacterStats myStats;
    private bool hasDamaged = false;

    public void SetupSummon(CharacterStats _stats) => myStats = _stats;

    private void Start()
    {
        rb.isKinematic = true;
    }

    private void Update()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(check.position, boxSize, whatIsPlayer);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null && !hasDamaged)
            {
                hit.GetComponent<Entity>().SetupKnockbackDir(transform);
                myStats.DoDamage(hit.GetComponent<CharacterStats>());
                hasDamaged = true;
            }
        }

        if (GroundBelow())
        {
            rb.velocity = new Vector2(0, 0);
            anim.SetTrigger("Explode");
        }
    }

    public void StartAcceleration()
    {
        rb.isKinematic = false;
    }

    private RaycastHit2D GroundBelow() => Physics2D.Raycast(transform.position, Vector2.down, (boxSize.y / 2) + 0.5f, whatIsGround);

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(check.position, boxSize);
        Gizmos.DrawLine(check.position, new Vector2(check.position.x, check.position.y - (boxSize.y / 2) - 0.5f));
    }

    private void SelfDestroy() => Destroy(gameObject);
}
