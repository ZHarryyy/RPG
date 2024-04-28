using System.Collections;
using UnityEngine;

public class Player : Entity
{
    public bool AirComboFinished = false; //�жϿ��������Ƿ������
    public bool isRed;
    public AltarOfThunderClawDestroyController altar;
    public float attackPressTime = 0.0f;

    public bool isBirth = true;
    public bool isDieFirstTime = true;
    public bool canTriggerBlackhole = false;
    public GameObject blackholeTrigger;

    [Header("Attack details")]
    public Vector2[] attackMovement;
    public Vector2[] airAttackMovement;
    public float counterAttackDuration = .2f;

    public bool isBusy { get; private set; }

    [Header("Move info")]
    public float moveSpeed = 12f;
    public float jumpForce;
    public float swordReturnImpact;
    private float defaultMoveSpeed;
    private float defaultJumpForce;

    [Header("Dash info")]
    public float dashSpeed;
    public float dashDuration;
    private float defaultDashSpeed;
    public float dashDir { get; private set; }

    public SkillManager skill { get; private set; }
    public GameObject sword { get; private set; }
    public PlayerFX fx { get; private set; }

    #region States
    public PlayerStateMachine stateMachine { get; private set; }

    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }
    public PlayerWallJumpState wallJumpState { get; private set; }
    public PlayerDashState dashState { get; private set; }

    public PlayerPrimaryAttackState primaryAttackState { get; private set; }
    public PlayerCounterAttackState counterAttackState { get; private set; }
    public PlayerJumpAttackState jumpAttackState { get; private set; }
    public PlayerAttackChargeState attackChargeState { get; private set; }
    public PlayerHeavyAttackState heavyAttackState { get; private set; }

    public PlayerDownAttackState downAttackState { get; private set; }

    public PlayerAimSwordState aimSwordState { get; private set; }
    public PlayerCatchSwordState catchSwordState { get; private set; }
    public PlayerBlackholeState blackholeState { get; private set; }

    public PlayerDeadState deadState { get; private set; }

    public PlayerBirthState birthState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        wallSlideState = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJumpState = new PlayerWallJumpState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");

        primaryAttackState = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
        counterAttackState = new PlayerCounterAttackState(this, stateMachine, "CounterAttack");
        jumpAttackState = new PlayerJumpAttackState(this, stateMachine, "JumpAttack");
        downAttackState = new PlayerDownAttackState(this, stateMachine, "AttackDownward");
        attackChargeState = new PlayerAttackChargeState(this, stateMachine, "AttackCharge");
        heavyAttackState = new PlayerHeavyAttackState(this, stateMachine, "HeavyAttack");

        aimSwordState = new PlayerAimSwordState(this, stateMachine, "AimSword");
        catchSwordState = new PlayerCatchSwordState(this, stateMachine, "CatchSword");
        blackholeState = new PlayerBlackholeState(this, stateMachine, "Jump");

        deadState = new PlayerDeadState(this, stateMachine, "Die");

        birthState = new PlayerBirthState(this, stateMachine, "Birth");
    }

    protected override void Start()
    {
        base.Start();

        fx = GetComponent<PlayerFX>();

        skill = SkillManager.instance;

        stateMachine.Initialize(idleState);

        defaultMoveSpeed = moveSpeed;
        defaultJumpForce = jumpForce;
        defaultDashSpeed = dashSpeed;
    }

    protected override void Update()
    {
        if (Time.timeScale == 0) return;

        base.Update();

        stateMachine.currentState.Update();

        CheckForDashInput();
        
        if (!isRed && Input.GetKeyDown(KeyCode.F) && skill.crystal.crystalUnlocked) skill.crystal.CanUseSkill();

        if (Input.GetKeyDown(KeyCode.H)) Inventory.instance.UseFlask();
    }

    public override void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
        moveSpeed = moveSpeed * (1 - _slowPercentage);
        jumpForce = jumpForce * (1 - _slowPercentage);
        dashSpeed = dashSpeed * (1 - _slowPercentage);
        anim.speed = anim.speed * (1 - _slowPercentage);

        Invoke("ReturnDefaultSpeed", _slowDuration);
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();

        moveSpeed = defaultMoveSpeed;
        jumpForce = defaultJumpForce;
        dashSpeed = defaultDashSpeed;
    }

    public void AssignNewSword(GameObject _newSword)
    {
        sword = _newSword;
    }

    public void CatchTheSword()
    {
        stateMachine.ChangeState(catchSwordState);
        Destroy(sword);
    }

    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;

        yield return new WaitForSeconds(_seconds);

        isBusy = false;
    }

    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();
    
    private void CheckForDashInput()
    {
        if (IsWallDetected()) return;

        if (isRed)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                dashDir = Input.GetAxisRaw("Horizontal");

                if (dashDir == 0) dashDir = facingDir;

                stateMachine.ChangeState(dashState);
            }
        }
        else
        {
            if (skill.dash.dashUnlocked == false || 
                stateMachine.currentState == birthState ||
                stateMachine.currentState == deadState) return;//��ĳЩ״̬����dash���������ڵ���

            if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.instance.dash.CanUseSkill())
            {
                dashDir = Input.GetAxisRaw("Horizontal");

                if (dashDir == 0) dashDir = facingDir;

                stateMachine.ChangeState(dashState);
            }
        }
    }

    public override void Die()
    {
        base.Die();

        stateMachine.ChangeState(deadState);
    }

    protected override void SetupZeroKnockbackPower()
    {
        knockbackPower = new Vector2(0, 0);
    }
}
