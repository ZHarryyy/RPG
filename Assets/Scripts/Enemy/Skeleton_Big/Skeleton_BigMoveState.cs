public class Skeleton_BigMoveState : Skeleton_BigGroundedState
{
    public Skeleton_BigMoveState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton_Big _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        enemy.SetVelocity(enemy.moveSpeed * enemy.facingDir, rb.velocity.y);

        if(enemy.IsWallDetected())
        {
            enemy.Flip();
            stateMachine.ChangeState(enemy.idleState);
        }
        else if(!enemy.IsGroundDetected())
        {
            stateMachine.ChangeState(enemy.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
