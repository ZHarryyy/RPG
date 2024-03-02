public class Enemy_Ooze : Enemy
{
    #region States
    public OozeIdleState idleState { get; private set; }
    public OozeMoveState moveState { get; private set; }
    public OozeBattleState battleState { get; private set; }
    public OozeAttackState attackState { get; private set; }

    public OozeStunnedState stunnedState { get; private set; }
    public OozeDeadState deadState { get; private set; }
    #endregion


    protected override void Awake()
    {
        base.Awake();

        SetupDefaultFacingDir(-1);

        idleState = new OozeIdleState(this, stateMachine, "Idle", this);
        moveState = new OozeMoveState(this, stateMachine, "Move", this);
        battleState = new OozeBattleState(this, stateMachine, "Move", this);
        attackState = new OozeAttackState(this, stateMachine, "Attack", this);
        stunnedState = new OozeStunnedState(this, stateMachine, "Stunned", this);
        deadState = new OozeDeadState(this, stateMachine, "Idle", this);
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }

    public override bool CanBeStunned()
    {
        if (base.CanBeStunned())
        {
            stateMachine.ChangeState(stunnedState);
            return true;
        }

        return false;
    }

    public override void Die()
    {
        CloseCounterAttackWindow();

        base.Die();

        stateMachine.ChangeState(deadState);
    }
}
