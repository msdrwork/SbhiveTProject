using System;

public abstract class State<TState> where TState : Enum
{
    public StateMachine<TState> stateMachine;

    public State(StateMachine<TState> stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public abstract void OnEnter();

    public abstract void OnUpdate();
}
