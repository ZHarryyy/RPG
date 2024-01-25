using UnityEngine;

public class PlayerAnimEventsTemp : MonoBehaviour
{
    private PlayerTemp player;

    private void Start()
    {
        player = GetComponentInParent<PlayerTemp>();
    }

    private void AnimationTrigger()
    {
        player.AttackOver();
    }
}
