using System.Collections;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();

    private void AnimationTrigger()
    {
        player.AnimationTrigger();
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null && !hit.GetComponent<Enemy>().isDead)
            {
                EnemyStats _target = hit.GetComponent<EnemyStats>();

                if (_target != null) player.stats.DoDamage(_target);

                AudioManager.instance.PlaySFX(35, null);

                player.fx.ScreenShake(player.fx.shakeNormalDamage);

                if (Inventory.instance != null)
                {
                    ItemData_Equipment weaponData = Inventory.instance.GetEquipment(EquipmentType.Weapon);

                    if (weaponData != null) weaponData.Effect(_target.transform);
                }
            }
            else
            {
                AudioManager.instance.PlaySFX(34, null);
            }
        }
    }

    private void ThrowSword()
    {
        SkillManager.instance.sword.CreateSword();
    }

    private void AnimationStop()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null && !hit.GetComponent<Enemy>().isDead) StartCoroutine(StopAnimation());
        }
    }

    private IEnumerator StopAnimation()
    {
        float animationStopDuration = 0.15f;

        player.anim.enabled = false;

        yield return new WaitForSeconds(animationStopDuration);

        player.anim.enabled = true;
    }
}
