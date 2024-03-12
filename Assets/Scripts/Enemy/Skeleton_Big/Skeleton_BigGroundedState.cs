using UnityEngine;

public class Skeleton_BigGroundedState : EnemyState
{
    protected Enemy_Skeleton_Big enemy;
    protected Transform player;

    public Skeleton_BigGroundedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton_Big _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player.transform;
    }

    public override void Update()
    {
        base.Update();

        if ((enemy.IsPlayerDetected() && !enemy.IsWallDetected() && enemy.IsGroundDetected()) || (Vector2.Distance(enemy.transform.position, player.transform.position) < enemy.agroDistance && enemy.IsGroundDetected()) || enemy.getHitted) stateMachine.ChangeState(enemy.battleState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
