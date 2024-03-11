using UnityEngine;

public class DangerZone : MonoBehaviour
{    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.collider.GetComponent<Player>();

        if (player != null)
        {
            player.GetComponent<Entity>().SetupKnockbackDir(transform);
            this.GetComponent<EnemyStats>().DoDamage(player.GetComponent<CharacterStats>());
            this.GetComponent<EnemyStats>().DoMagicDamage(player.GetComponent<CharacterStats>());
        }
    }
}
