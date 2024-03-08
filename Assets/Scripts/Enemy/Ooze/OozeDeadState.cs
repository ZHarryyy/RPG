using UnityEngine;

public class OozeDeadState : EnemyState
{
    private Enemy_Ooze enemy;

    public OozeDeadState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Ooze _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.isDead = true;

        Physics2D.IgnoreCollision(enemy.GetComponent<Collider2D>(), PlayerManager.instance.player.GetComponent<Collider2D>(), true);
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
