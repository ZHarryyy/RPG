using System.Collections;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    private Player player;   
    public SpriteRenderer spriteRenderer;
    public float invincibleDuration = 2f;
    public float blinkDuration = 0.1f;
    public float minAlpha = 0.4f;
    public float maxAlpha = 1f;
    private float elapsedTime;
    private bool isFadingOut;

    protected override void Start()
    {
        base.Start();

        player = GetComponent<Player>();
    }

    protected override void Update()
    {
        if (isInvincible)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= blinkDuration)
            {
                if (isFadingOut)
                {
                    spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, maxAlpha);
                    isFadingOut = false;
                }
                else
                {
                    spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, minAlpha);
                    isFadingOut = true;
                }

                elapsedTime = 0f;
            }
        }
        else
        {
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, maxAlpha);
        }
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
    }

    protected override void Die()
    {
        base.Die();

        player.Die();

        GameManager.instance.lostSoulAmount = PlayerManager.instance.currency;
        PlayerManager.instance.currency = 0;

        GetComponent<PlayerItemDrop>()?.GenerateDrop();
    }

    protected override void DecreaseHealthBy(int _damage)
    {
        base.DecreaseHealthBy(_damage);

        player.SetupKnockbackPower(new Vector2(10, 6));
        player.fx.ScreenShake(player.fx.shakeHighDamage);
        InvincibleBlink();
        player.stats.MakeInvincible(true);

        ItemData_Equipment currentArmor = Inventory.instance.GetEquipment(EquipmentType.Armor);

        if (currentArmor != null) currentArmor.Effect(player.transform);

        StartCoroutine(DisableInvincibilityAfterDuration());
    }

    private void InvincibleBlink()
    {
        if(isInvincible)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= blinkDuration)
            {
                if (isFadingOut)
                {
                    spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, maxAlpha);
                    isFadingOut = false;
                }
                else
                {
                    spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, minAlpha);
                    isFadingOut = true;
                }

                elapsedTime = 0f;
            }
            else
            {
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, maxAlpha);
            }
        }
    }

    private IEnumerator DisableInvincibilityAfterDuration()
    {
        yield return new WaitForSeconds(1.5f);

        player.stats.MakeInvincible(false);
    }

    public override void OnEvasion()
    {
        player.skill.dodge.CreateMirageOnDodge();
    }

    public void CloneDoDamage(CharacterStats _targetStats, float _multiplier)
    {
        if (TargetCanAvoidAttack(_targetStats)) return;

        int totalDamage = damage.GetValue() + strength.GetValue();

        if (_multiplier > 0) totalDamage = Mathf.RoundToInt(totalDamage * _multiplier);

        if (CanCrit())
        {
            totalDamage = CalculateCriticalDamage(totalDamage);
        }

        totalDamage = CheckTargetArmor(_targetStats, totalDamage);

        _targetStats.TakeDamage(totalDamage);

        DoMagicDamage(_targetStats);
    }
}
