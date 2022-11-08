using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderScareBoidState : State
{
    public bool isComplete = false;
    public WanderScareBoidState(Agent owner, StateManager sm) : base(owner, sm)
    {
    }

    public override void Enter()
    {
        Debug.Log("Entering scare boid.");
        isComplete = true;
        agent.agentAnimator.SetTrigger("Attack");
    }

    public override void ReEnter()
    {
        Debug.Log("Re-entering scare boid.");
        IsReEntering = false;
        agent.agentAnimator.SetTrigger("Attack");
        currentReEnteringTime -= Time.deltaTime;
    }

    public override void Execute()
    {
        Debug.Log("Executing scare boid.");
        currentReEnteringTime -= Time.deltaTime;
    }

    public override void Exit()
    {
        Debug.Log("Exiting scare boid.");
        isComplete = false;
    }
}
