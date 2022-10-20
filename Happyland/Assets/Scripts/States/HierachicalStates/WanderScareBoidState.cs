using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderScareBoidState : State
{
    public WanderScareBoidState(Agent owner, StateManager sm) : base(owner, sm)
    {
    }

    public override void Enter()
    {
        Debug.Log("Entering scare boid.");
    }

    public override void ReEnter()
    {
        Debug.Log("Re-entering scare boid.");
    }

    public override void Execute()
    {
        Debug.Log("Executing scare boid.");
    }

    public override void Exit()
    {
        Debug.Log("Exiting scare boid.");
    }
}