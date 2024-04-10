using UnityEngine;
public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (player.IsWallDetected() && !player.isRed)
        {
            player.AirComboFinished = false;
            stateMachine.ChangeState(player.wallSlideState);
        }

        if (player.IsGroundDetected()) stateMachine.ChangeState(player.idleState);

        if ((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) && Input.GetKeyDown(KeyCode.J))
        {
            stateMachine.ChangeState(player.downAttackState);
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            if(player.isRed)
            {
                stateMachine.ChangeState(player.primaryAttackState);
            }
            else
            {
                if (!player.AirComboFinished)
                {
                    stateMachine.ChangeState(player.jumpAttackState);
                }
            }
        }

        if (xInput != 0 && !player.IsWallDetected()) player.SetVelocity(player.moveSpeed * .8f * xInput, rb.velocity.y);
    }
}
