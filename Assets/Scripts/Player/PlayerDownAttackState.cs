public class PlayerDownAttackState : PlayerState
{
    private bool alreadyDown;
    public PlayerDownAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
        downwardAttackCharged = false;//����ʱ��������һ��������������ɺ����׹
        alreadyDown = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(downwardAttackCharged)
        {
            player.SetVelocity(xInput * player.moveSpeed, -10);

            //Ϊ����ز��ظ����Ŷ���������alreadyDown�ж�
            if (!alreadyDown && player.IsGroundDetected())
            {
                player.fx.PlayDustFX();
                alreadyDown = true;
            }
        }
        else
        {
            player.SetZeroVelocity();
        }



        if (triggerCalled)
        {
            if (player.IsGroundDetected())
            {
                stateMachine.ChangeState(player.idleState);
            }
            else
            {
                stateMachine.ChangeState(player.airState);
            }
        }
    }
}

