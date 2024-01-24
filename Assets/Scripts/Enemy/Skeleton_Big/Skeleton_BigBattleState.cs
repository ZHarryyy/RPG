using UnityEngine;

public class Skeleton_BigBattleState : EnemyState
{
    private Transform player;
    private Enemy_Skeleton_Big enemy;
    private int moveDir;

    public Skeleton_BigBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton_Big _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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
        
        if(enemy.IsPlayerDetected())
        {
            stateTimer = enemy.battleTime;

            if(enemy.IsPlayerDetected().distance < enemy.attackDistance)
            {
                if(CanAttack()) stateMachine.ChangeState(enemy.attackState);
            }
        }
        else
        {
            if(stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > 10) stateMachine.ChangeState(enemy.idleState);
        }

        if(player.position.x > enemy.transform.position.x + .3f) moveDir = 1;
        else if(player.position.x < enemy.transform.position.x - .3f) moveDir = -1;

        if(enemy.IsGroundDetected())
        {
            enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.velocity.y);
        }
        else
        {
            stateMachine.ChangeState(enemy.idleState);
        }

        if(enemy.IsWallDetected()) stateMachine.ChangeState(enemy.idleState);
    }

    public override void Exit()
    {
        base.Exit();
    }

    private bool CanAttack()
    {
        if(Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown)
        {
            enemy.lastTimeAttacked = Time.time;
            return true;
        }

        return false;
    }
}
