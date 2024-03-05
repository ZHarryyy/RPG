public class DeathBringerTeleportState : EnemyState
{
    private Enemy_DeathBringer enemy;

    public DeathBringerTeleportState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_DeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.FindPosition();

        stateTimer = 1;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0) stateMachine.ChangeState(enemy.idleState);
    }
}
