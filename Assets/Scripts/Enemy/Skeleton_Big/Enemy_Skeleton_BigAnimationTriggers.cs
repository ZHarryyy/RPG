using UnityEngine;

public class Enemy_Skeleton_BigAnimationTriggers : MonoBehaviour
{
    private Enemy_Skeleton_Big enemy => GetComponentInParent<Enemy_Skeleton_Big>();

    private void AnimationFinishTrigger()
    {
        enemy.AnimationFinishTrigger();
    }
}
