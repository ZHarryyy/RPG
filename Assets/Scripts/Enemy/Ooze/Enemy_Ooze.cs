using UnityEngine;

public enum OozeType { big, medium, small }

public class Enemy_Ooze : Enemy
{
    [Header("Ooze specific")]
    [SerializeField] private OozeType oozeType;
    [SerializeField] private int oozeToCreate;
    [SerializeField] private GameObject oozePrefab;
    [SerializeField] private Vector2 minCreationVelocity;
    [SerializeField] private Vector2 maxCreationVelocity;

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
        deadState = new OozeDeadState(this, stateMachine, "Dead", this);
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

        if (oozeType == OozeType.small) return;
        else CreateOozes(oozeToCreate, oozePrefab);
    }

    private void CreateOozes(int _amountOfOoze, GameObject _oozePrefab)
    {
        for (int i = 0; i < _amountOfOoze; i++)
        {
            GameObject newOoze = Instantiate(_oozePrefab, transform.position, Quaternion.identity);

            newOoze.GetComponent<Enemy_Ooze>().SetupOoze(facingDir);
        }
    }

    public void SetupOoze(int _facingDir)
    {
        if (_facingDir != facingDir) Flip();

        float xVelocity = Random.Range(minCreationVelocity.x, maxCreationVelocity.x);
        float yVelocity = Random.Range(minCreationVelocity.y, maxCreationVelocity.y);

        isKnocked = true;

        GetComponent<Rigidbody2D>().velocity = new Vector2(xVelocity * _facingDir, yVelocity);

        Invoke("CancelKnockback", 1.5f);
    }

    private void CancelKnockback() => isKnocked = false;
}
