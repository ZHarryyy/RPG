using UnityEngine;

public class BatDeadState : EnemyState
{
    private Enemy_Bat enemy;

    public BatDeadState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Bat _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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

        if (triggerCalled) enemy.SelfDestroy();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
