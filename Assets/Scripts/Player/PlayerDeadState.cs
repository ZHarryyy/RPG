using UnityEngine;

public class PlayerDeadState : PlayerState
{
    public PlayerDeadState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();

        if (player.isRed) GameObject.Find("Canvas").GetComponent<Level1UI>().SwitchOnEndScreen();
        else GameObject.Find("Canvas").GetComponent<UI>().SwitchOnEndScreen();
    }

    public override void Update()
    {
        base.Update();

        player.SetZeroVelocity();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
