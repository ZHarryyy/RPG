using UnityEngine;

public class ExecutionerIdleState : EnemyState
{
    private Enemy_Executioner enemy;
    private Transform player;

    public ExecutionerIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Executioner _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = enemy.idleTime;

        player = PlayerManager.instance.player.transform;
    }

    public override void Update()
    {
        base.Update();

        if (Vector2.Distance(player.transform.position, enemy.transform.position) < 10) enemy.bossFightBegun = true;

        if (Input.GetKeyDown(KeyCode.V)) stateMachine.ChangeState(enemy.teleportState);

        if (stateTimer < 0 && enemy.bossFightBegun) stateMachine.ChangeState(enemy.battleState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
