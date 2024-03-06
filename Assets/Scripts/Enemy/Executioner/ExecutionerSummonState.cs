using UnityEngine;

public class ExecutionerSummonState : EnemyState
{
    private Enemy_Executioner enemy;

    private int amountOfSummons;
    private float summonTimer;

    public ExecutionerSummonState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Executioner _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        amountOfSummons = enemy.amountOfSummons;

        summonTimer = .5f;
    }

    public override void Update()
    {
        base.Update();

        summonTimer -= Time.deltaTime;

        if (CanSummon()) enemy.Summon();

        if (amountOfSummons <= 0) stateMachine.ChangeState(enemy.teleportState);
    }

    public override void Exit()
    {
        base.Exit();

        enemy.lastTimeSummon = Time.time;
    }

    private bool CanSummon()
    {
        if (amountOfSummons > 0 && summonTimer < 0)
        {
            amountOfSummons--;
            summonTimer = enemy.summonCooldown;
            return true;
        }

        return false;
    }
}
