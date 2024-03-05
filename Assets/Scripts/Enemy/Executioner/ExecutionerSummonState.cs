public class ExecutionerSummonState : EnemyState
{
    private Enemy_Executioner enemy;

    public ExecutionerSummonState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Executioner _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }
}
