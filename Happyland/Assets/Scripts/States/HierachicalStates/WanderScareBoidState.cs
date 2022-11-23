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
        isComplete = true;
        agent.agentAnimator.SetTrigger("Scare");
    }

    public override void ReEnter()
    {
        IsReEntering = false;
        agent.agentAnimator.SetTrigger("Scare");
        currentReEnteringTime -= Time.deltaTime;
        isComplete = true;
    }

    public override void Execute()
    {
        currentReEnteringTime -= Time.deltaTime;
        isComplete = true;
    }

    public override void Exit()
    {
    }
}
