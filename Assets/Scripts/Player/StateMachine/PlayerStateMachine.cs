using UnityEngine;

public class PlayerStateMachine
{
    public PlayerState CurrentState {get; private set;}

    public void Initialize(PlayerState startState)
    {
        CurrentState = startState;
    }

    public void ChangeState(PlayerState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }
}
