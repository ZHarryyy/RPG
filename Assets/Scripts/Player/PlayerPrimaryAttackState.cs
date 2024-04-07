using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    public int comboCounter { get; private set; }

    private float lastTimeAttacked;
    private float comboWindow = 2;

    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // AudioManager.instance.PlaySFX(1);

        xInput = 0;

        if (player.isRed)
        {
            if (comboCounter > 1 || Time.time >= lastTimeAttacked + comboWindow) comboCounter = 0;
        }
        else
        {
            if (comboCounter > 2 || Time.time >= lastTimeAttacked + comboWindow) comboCounter = 0;
        }

        player.anim.SetInteger("ComboCounter", comboCounter);

        float attackDir = player.facingDir;
        if (xInput != 0) attackDir = xInput;

        player.SetVelocity( player.attackMovement[comboCounter].x * attackDir, rb.velocity.y + player.attackMovement[comboCounter].y);

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

        if (stateTimer < 0 && player.IsGroundDetected())
        {
            player.SetVelocity(0, rb.velocity.y);
        }
        else 
        {
            player.SetVelocity(xInput * player.moveSpeed, rb.velocity.y);//继续上一帧的速度进行运动 
        }

        if (triggerCalled)
        {
            if(player.IsGroundDetected())
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
