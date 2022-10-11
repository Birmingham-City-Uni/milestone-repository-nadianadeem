using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    protected Agent agent;
    protected StateManager sm;

    protected State(Agent _agent, StateManager _sm)
    {
        this.agent = _agent;
        this.sm = _sm;

    }

    public abstract void Enter();

    public abstract void Execute();

    public abstract void Exit();
}