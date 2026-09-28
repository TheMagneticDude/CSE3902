using System;
using System.Collections.Generic;

public class StateMachine<TState> where TState : Enum
{
    //Dictionary for storing states
    private readonly Dictionary<TState, IState> states = new();
    public IState? CurrentState {get; private set;}

    public void AddState(TState key, IState state)
    {
        states[key] = state;
    }

    public void ChangeState(TState key)
    {
        //State has not been in dictionary
        if(!states.TryGetValue(key, out var nextState))
        {
            //exception was from ChatGPT
            throw new InvalidOperationException($"State {key} has not been registered");
        }
        //Leave 
        if(!ReferenceEquals(CurrentState, nextState))
        {
            CurrentState?.Exit();
            CurrentState = nextState;
            CurrentState.Enter();
        }
    }

    public void Update(float deltaTime)
    {
        CurrentState?.Update(deltaTime);
    }

}