using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieState : State
{
    public DieState(Agent owner, StateManager sm) : base(owner, sm)
    {
    }

    public override void Enter()
    {
        Debug.Log("Entering Die");
    }

    public override void ReEnter()
    {
        Debug.Log("Entering Idle");
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
