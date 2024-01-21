using UnityEngine;

public class Enemy_Skeleton_BigAnimationTriggers : MonoBehaviour
{
    private Enemy_Skeleton_Big enemy => GetComponentInParent<Enemy_Skeleton_Big>();

    private void AnimationFinishTrigger()
    {
        enemy.AnimationFinishTrigger();
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);

        foreach(var hit in colliders)
        {
            if(hit.GetComponent<Player>() != null) hit.GetComponent<Player>().Damage();
        }
    }
}
