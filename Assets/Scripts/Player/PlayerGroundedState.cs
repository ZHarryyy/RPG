using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    private float chargeThreshold = 0.2f;

    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (player.isBirth) stateMachine.ChangeState(player.birthState);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (!player.isRed && Input.GetKeyDown(KeyCode.R) && player.skill.blackhole.blackholeUnlocked)
        {
            if (player.skill.blackhole.cooldownTimer > 0)
            {
                player.fx.CreatePopUpText("Skill is on Cooldown");
                return;
            }

            stateMachine.ChangeState(player.blackholeState);
        }

        if (!player.isRed && Input.GetKeyDown(KeyCode.Mouse1) && HasNoSword() && player.skill.sword.swordUnlocked && !player.isBusy) stateMachine.ChangeState(player.aimSwordState);

        if (!player.isRed && Input.GetKeyDown(KeyCode.Q) && player.skill.parry.parryUnlocked) stateMachine.ChangeState(player.counterAttackState);

        if(player.isRed)//小红帽没有蓄力攻击
        {
            if (Input.GetKeyDown(KeyCode.J)) stateMachine.ChangeState(player.primaryAttackState);
        }
        else
        {
            //判断蓄力攻击
            if (Input.GetKey(KeyCode.J))//持续时间
            {
                player.attackPressTime += Time.deltaTime;
                if (player.attackPressTime >= chargeThreshold)//若按下的持续时间超过阈值则充能
                {
                    stateMachine.ChangeState(player.attackChargeState);
                }
            }
        }

        //若在阈值内松开攻击键则判定为普攻
        if (Input.GetKeyUp(KeyCode.J))
        {
            stateMachine.ChangeState(player.primaryAttackState);//没有转换到蓄力状态则为普攻
            player.attackPressTime = 0.0f;
        }

        if (!player.IsGroundDetected()) stateMachine.ChangeState(player.airState);

        if (Input.GetKeyDown(KeyCode.Space) && player.IsGroundDetected()) stateMachine.ChangeState(player.jumpState);
    }

    private bool HasNoSword()
    {
        if (!player.sword) return true;

        player.sword.GetComponent<Sword_Skill_Controller>().ReturnSword();

        return false;
    }
}
