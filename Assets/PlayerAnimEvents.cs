using UnityEngine;

public class PlayerAnimEvents : MonoBehaviour
{
    private Player player;

    private void Start()
    {
        player = GetComponentInParent<Player>();
    }

    private void AnimationTrigger()
    {
        player.AttackOver();
    }
}
