using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpAttackState : PlayerState
{
    public int comboCounter = 0;

    private float lastTimeAttacked;
    private float comboWindow = 0.5f;

    // Start is called before the first frame update
    public PlayerJumpAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        xInput = 0;
        if (comboCounter == 2)
        {
            player.AirComboFinished = true;//完成combo则air和jump状态不会再跳转到空中攻击
        }
        else if (comboCounter > 2 || Time.time >= lastTimeAttacked + comboWindow)
        {
            comboCounter = 0;
        }

        player.anim.SetInteger("AirComboCounter", comboCounter);

        float attackDir = player.facingDir;
        if (xInput != 0) attackDir = xInput;

        player.SetVelocity(player.airAttackMovement[comboCounter].x * attackDir, rb.velocity.y + player.airAttackMovement[comboCounter].y);

        stateTimer = .1f;
    }

    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine("BusyFor", .15f);

        comboCounter++;
        lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();


        player.SetVelocity(xInput * player.moveSpeed * 0.5f, 0);


        if (triggerCalled)
        {
            if (player.IsGroundDetected())
            {
                stateMachine.ChangeState(player.idleState);
            }
            else
            {
                //为了兼容空中攻击状态，如果在地面会再转状态
                stateMachine.ChangeState(player.airState);
            }
        }

    }
}
