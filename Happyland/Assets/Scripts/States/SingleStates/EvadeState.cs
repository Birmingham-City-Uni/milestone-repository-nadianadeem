using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvadeState : State
{
    public EvadeState(Agent owner, StateManager sm) : base(owner, sm)
    {
    }

    public override void Enter()
    {
        Debug.Log("Entering Evade");
    }

    public override void ReEnter()
    {
        Debug.Log("Entering Idle");
    }

    public override void Execute()
    {
        Debug.Log("Executing Evade");
    }

    public override void Exit()
    {
        Debug.Log("Exiting Evade");
    }
}
