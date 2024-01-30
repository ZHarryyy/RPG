using UnityEngine;

public class Blackhole_Skil : Skill
{
    [SerializeField] private int amountOfAttacks;
    [SerializeField] private float cloneAttackCooldown;
    [Space]
    [SerializeField] private GameObject blackHolePrefab;
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;

    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();

        GameObject newBlackHole = Instantiate(blackHolePrefab);

        Blackhole_Skill_Controller newBlackHoleScript = newBlackHole.GetComponent<Blackhole_Skill_Controller>();

        newBlackHoleScript.SetupBlackhole(maxSize, growSpeed, shrinkSpeed, amountOfAttacks, cloneAttackCooldown);
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }
}
