using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    protected Agent agent;
    protected StateManager sm;
    public bool IsReEntering = false;
    public float currentReEnteringTime;
    public float ReEnteringTime = 5.0f;
    protected Vector3 lastLocation;

    protected State(Agent _agent, StateManager _sm)
    {
        this.agent = _agent;
        this.sm = _sm;
    }

    public abstract void Enter();

    public abstract void ReEnter();

    public abstract void Execute();

    public abstract void Exit();
}