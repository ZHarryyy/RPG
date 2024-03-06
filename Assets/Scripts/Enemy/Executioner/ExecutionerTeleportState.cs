public class ExecutionerTeleportState : EnemyState
{
    private Enemy_Executioner enemy;

    public ExecutionerTeleportState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Executioner _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.stats.MakeInvincible(true);
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled)
        {
            if (enemy.CanSummon()) stateMachine.ChangeState(enemy.summonState);
            else stateMachine.ChangeState(enemy.battleState);
        }
    }

    public override void Exit()
    {
        base.Exit();

        enemy.stats.MakeInvincible(false);
    }
}
