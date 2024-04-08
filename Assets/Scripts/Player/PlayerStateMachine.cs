public class PlayerStateMachine
{
    public PlayerState currentState { get; private set; }
    public PlayerState lastState { get; private set; }

    public void Initialize(PlayerState _startState)
    {
        currentState = _startState;
        currentState.Enter();
    }

    public void ChangeState(PlayerState _newState)
    {
        lastState = currentState;
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }
}
