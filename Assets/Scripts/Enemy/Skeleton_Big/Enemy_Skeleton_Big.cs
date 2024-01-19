public class Enemy_Skeleton_Big : Enemy
{
    #region States
    public Skeleton_BigIdleState idleState { get; private set; }
    public Skeleton_BigMoveState moveState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        idleState = new Skeleton_BigIdleState(this, stateMachine, "Idle", this);
        moveState = new Skeleton_BigMoveState(this, stateMachine, "Move", this);
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
    }
}
