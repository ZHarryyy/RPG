using UnityEngine;

public class ExecutionerDeadState : EnemyState
{
    private Enemy_Executioner enemy;

    public ExecutionerDeadState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Executioner _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.anim.SetBool(enemy.lastAnimBoolName, true);
        enemy.anim.speed = 0;
        enemy.cd.enabled = false;

        stateTimer = .15f;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer > 0) rb.velocity = new Vector2(0, 10);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
