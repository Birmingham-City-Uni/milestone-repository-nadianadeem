using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvadeState : State
{
    Agent owner;
    public EvadeState(Agent owner)
    {
        this.owner = owner;
    }

    public override void Enter()
    {
        Debug.Log("Entering Evade");
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
