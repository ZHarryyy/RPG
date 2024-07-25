using UnityEngine;

public class PlayerBirthState : PlayerState
{
    private bool isDelayedStateChangeStarted = false;
    private float delayTimer = 0f;
    private float delayDuration = 2f;

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

        Transform childTransform = player.transform.Find("Container");

        GameObject childContainerObject = null;

        if (childTransform != null) childContainerObject = childTransform.gameObject;

        if(childContainerObject != null)
        {
            childContainerObject.SetActive(true);
        }
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer <= 0 && player.isRed)
        {
            player.isBirth = false;
            stateMachine.ChangeState(player.idleState);
        }
        else if (!player.isRed && player.altar.isActivate && player.altar.enable)
        {
            player.anim.speed = 1;

            if (!isDelayedStateChangeStarted)
            {
                isDelayedStateChangeStarted = true;
                delayTimer = delayDuration;
            }

            if (isDelayedStateChangeStarted)
            {
                delayTimer -= Time.deltaTime;
                if (delayTimer <= 0f)
                {
                    player.isBirth = false;
                    stateMachine.ChangeState(player.idleState);
                }
            }
        }
        else if(stateTimer <= 0 && !player.isRed && !player.altar.enable)
        {
            player.isBirth = false;
            stateMachine.ChangeState(player.idleState);
        }
    }
}
