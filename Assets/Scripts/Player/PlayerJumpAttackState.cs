using System.Linq;
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
            player.AirComboFinished = true;//���combo��air��jump״̬��������ת�����й���
        }
        else if (comboCounter > 2 || Time.time >= lastTimeAttacked + comboWindow)
        {
            comboCounter = 0;
        }

        player.anim.SetInteger("AirComboCounter", comboCounter);

        float attackDir = player.facingDir;
        if (xInput != 0) attackDir = xInput;

        if (comboCounter < player.airAttackMovement.Count()) {
            player.SetVelocity(player.airAttackMovement[comboCounter].x * attackDir, rb.velocity.y + player.airAttackMovement[comboCounter].y);
        } else {
            Debug.LogFormat("[error] comboCounter out of index. index: " + comboCounter + ", array count " + player.airAttackMovement.Count());
        }

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
                //Ϊ�˼��ݿ��й���״̬������ڵ������ת״̬
                stateMachine.ChangeState(player.airState);
            }
        }

    }
}
