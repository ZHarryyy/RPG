using UnityEngine;

public class NeedlingController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>() != null)
        {
            rb.simulated = true;
        }
    }
}
