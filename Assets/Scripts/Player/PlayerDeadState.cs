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

        if (player.isRed)
        {
            GameObject.Find("Canvas").GetComponent<Level0UI>().SwitchOnEndScreen();
        }
        else if (!player.isRed && player.isDieFirstTime)
        {
            player.isDieFirstTime = false;
            player.blackholeTrigger.SetActive(true);
            player.canTriggerBlackhole = true;
            player.stats.currentHealth = player.stats.maxHealth.GetValue();
        }
        else if (!player.isRed && !player.isDieFirstTime)
        {
            GameObject.Find("Canvas").GetComponent<UI>().SwitchOnEndScreen();
        }
    }

    public override void Update()
    {
        base.Update();

        player.SetZeroVelocity();

        player.rb.isKinematic = true;

        if (player.canTriggerBlackhole && Input.GetKeyDown(KeyCode.R))
        {
            stateMachine.ChangeState(player.blackholeState);
        }
    }

    public override void Exit()
    {
        base.Exit();

        player.rb.isKinematic = false;
    }
}
