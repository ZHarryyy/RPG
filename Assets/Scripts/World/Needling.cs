using UnityEngine;

public class Needling : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private string targetLayerName = "Player";
    private Rigidbody2D rb;
    private CharacterStats myStats;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myStats = GetComponent<EnemyStats>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(targetLayerName))
        {
            myStats.DoDamage(collision.GetComponent<CharacterStats>());
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            StuckInto(collision);
        }
    }

    private void StuckInto(Collider2D collision)
    {
        GetComponent<BoxCollider2D>().enabled = false;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }
}
