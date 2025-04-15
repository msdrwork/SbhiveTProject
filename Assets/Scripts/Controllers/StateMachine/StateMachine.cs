using UnityEngine;
using System.Collections.Generic;
using System;

public class StateMachine<TState> : IUpdateable where TState : Enum
{
    private Dictionary<TState, State<TState>> States;
    private State<TState> currentState;

    private bool isActive;
    private bool isDirty;

    public StateMachine()
    {
        States = new Dictionary<TState, State<TState>>();
        isActive = false;
    }

    public void AddState(TState stateId, State<TState> newState)
    {
        if (!States.ContainsKey(stateId))
        {
            States.Add(stateId, newState);
        }
        else
        {
            Debug.LogWarning("[StateMachine]: Cant add same state twice");
        }
    }

    public void RemoveState(TState stateId)
    {
        if (States.ContainsKey(stateId))
        {
            States.Remove(stateId);
        }
    }

    public void ChangeState(TState stateId)
    {
        currentState = States[stateId];
        currentState.OnEnter();
    }

    public void OnUpdate()
    {
        if (isActive && currentState != null)
        {
            currentState.OnUpdate();
        }
    }

    public void SetActive(bool isActive)
    {
        this.isActive = isActive;
    }

    public void Destroy()
    {
        States.Clear();
    }
}
