using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();


        player.SetZeroVelocity();
        player.AirComboFinished = false;//重置空中连段
        //player.jumpState.Jumping = false;//落地后重置可跳跃
        player.gameObject.GetComponent<Rigidbody2D>().bodyType =  RigidbodyType2D.Kinematic;

    }

    public override void Exit()
    {
        base.Exit();
        player.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

    }

    public override void Update()
    {
        base.Update();

        if (xInput == player.facingDir && player.IsWallDetected()) return;

        if (xInput != 0 && !player.isBusy) stateMachine.ChangeState(player.moveState);
    }
}
