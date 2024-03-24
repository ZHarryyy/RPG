using UnityEngine;

public class PlayerBirthState : PlayerState
{
    public PlayerBirthState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.SetZeroVelocity();

        stateTimer = 2.4f;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        
        if(stateTimer <= 0)
        {
            player.isBirth = false;
            stateMachine.ChangeState(player.idleState);
        }
    }
}
