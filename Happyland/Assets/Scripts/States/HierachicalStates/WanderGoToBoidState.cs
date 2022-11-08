using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderGoToBoidState : State
{
    public WanderGoToBoidState(Agent owner, StateManager sm) : base(owner, sm)
    {
    }

    public override void Enter()
    {
        Debug.Log("Entering go to boid.");
    }

    public override void ReEnter()
    {
        Debug.Log("Re-entering go to boid.");
        IsReEntering = false;
    }

    public override void Execute()
    {
        Debug.Log("Executing go to boid.");
        agent.Move(10f, agent.sensor.info.point);
        agent.agentAnimator.SetBool("IsMoving", true);
    }

    public override void Exit()
    {
        Debug.Log("Exiting go to boid.");
    }
}
