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

        if(player.isRed)//С��ñû����������
        {
            if (Input.GetKeyDown(KeyCode.J)) stateMachine.ChangeState(player.primaryAttackState);
        }
        else
        {
            //�ж���������
            if (Input.GetKey(KeyCode.J))//����ʱ��
            {
                player.attackPressTime += Time.deltaTime;
                if (player.attackPressTime >= chargeThreshold)//�����µĳ���ʱ�䳬����ֵ�����
                {
                    stateMachine.ChangeState(player.attackChargeState);
                }
            }
        }

        //������ֵ���ɿ����������ж�Ϊ�չ�
        if (Input.GetKeyUp(KeyCode.J))
        {
            stateMachine.ChangeState(player.primaryAttackState);//û��ת��������״̬��Ϊ�չ�
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
