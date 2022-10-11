using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnState : State
{
    public SpawnState(Agent owner, StateManager sm) : base(owner, sm)
    {
    }

    public override void Enter()
    {
        Debug.Log("Entering Spawn");
    }

    public override void Execute()
    {
        Debug.Log("Executing Spawn");
    }

    public override void Exit()
    {
        Debug.Log("Exiting Spawn");
    }
}
