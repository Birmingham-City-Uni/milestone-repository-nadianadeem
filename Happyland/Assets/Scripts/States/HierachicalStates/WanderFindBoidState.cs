using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderFindBoidState : State
{
    public WanderFindBoidState(Agent owner, StateManager sm) : base(owner, sm)
    {
    }

    public override void Enter()
    {
        Debug.Log("Entering find boid.");
    }

    public override void ReEnter()
    {
        Debug.Log("Re-entering find boid.");
    }

    public override void Execute()
    {
        Debug.Log("Executing find boid.");
    }

    public override void Exit()
    {
        Debug.Log("Exiting find boid.");
    }
}
