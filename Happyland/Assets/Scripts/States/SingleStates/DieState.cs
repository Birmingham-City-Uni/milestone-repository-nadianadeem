using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieState : State
{
    Agent owner;
    public DieState(Agent owner)
    {
        this.owner = owner;
    }

    public override void Enter()
    {
        Debug.Log("Entering Die");
    }

    public override void Execute()
    {
        Debug.Log("Executing Die");
    }

    public override void Exit()
    {
        Debug.Log("Exiting Die");
    }
}
