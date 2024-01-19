public class Skeleton_BigIdleState : EnemyState
{
    private Enemy_Skeleton_Big enemy;

    public Skeleton_BigIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton_Big _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = enemy.idleTime;
    }

    public override void Update()
    {
        base.Update();

        if(stateTimer < 0) stateMachine.ChangeState(enemy.moveState);
                
        if(stateTimer < 0 && !enemy.IsGroundDetected())
        {
            enemy.Flip();
            stateMachine.ChangeState(enemy.moveState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
