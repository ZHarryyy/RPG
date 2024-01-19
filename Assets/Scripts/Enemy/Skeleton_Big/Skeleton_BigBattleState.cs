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

        player = GameObject.Find("Warrior").transform;
    }

    public override void Update()
    {
        base.Update();

        if(enemy.IsPlayerDetected().distance < enemy.attackDistance)
        {
            Debug.Log("I ATTACK");
            enemy.ZeroVelocity();
            return;
        }

        if(player.position.x > enemy.transform.position.x) moveDir = 1;
        else if(player.position.x < enemy.transform.position.x) moveDir = -1;

        enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
