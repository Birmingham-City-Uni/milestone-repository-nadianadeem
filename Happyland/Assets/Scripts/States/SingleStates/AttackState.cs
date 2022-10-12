using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    Agent owner;
    public AttackState(Agent owner, StateManager sm): base(owner, sm)
    {
    }

    public override void Enter()
    {
        Debug.Log("Entering Attack");
    }

    public override void ReEnter()
    {
        Debug.Log("Entering Idle");
    }

    public override void Execute()
    {
        Debug.Log("Executing Attack");
    }

    public override void Exit()
    {
        Debug.Log("Exiting Attack");
    }
}
