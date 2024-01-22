using UnityEngine;

public class Enemy_Skeleton_Big : Enemy
{
    #region States
    public Skeleton_BigIdleState idleState { get; private set; }
    public Skeleton_BigMoveState moveState { get; private set; }
    public Skeleton_BigBattleState battleState { get; private set; }
    public Skeleton_BigAttackState attackState { get; private set; }
    public SkeletonBigStunnedState stunnedState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        idleState = new Skeleton_BigIdleState(this, stateMachine, "Idle", this);
        moveState = new Skeleton_BigMoveState(this, stateMachine, "Move", this);
        battleState = new Skeleton_BigBattleState(this, stateMachine, "Move", this);
        attackState = new Skeleton_BigAttackState(this, stateMachine, "Attack", this);
        stunnedState = new SkeletonBigStunnedState(this, stateMachine, "Stunned", this);
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();

        if(Input.GetKeyDown(KeyCode.U)) stateMachine.ChangeState(stunnedState);
    }
}
