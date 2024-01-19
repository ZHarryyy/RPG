public class Skeleton_BigGroundedState : EnemyState
{
    protected Enemy_Skeleton_Big enemy;

    public Skeleton_BigGroundedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton_Big _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if(enemy.IsPlayerDetected()) stateMachine.ChangeState(enemy.battleState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
