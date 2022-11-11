using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class MoveAwayNode : ActionNode
{
    Animator agentAnimator;
    protected override void OnStart()
    {
        agentAnimator = agent.GetComponent<Animator>();
        agentAnimator.SetBool("IsMoving", true);
        agent.calculateOnce = true;
    }

    protected override void OnStop()
    {
    }

    protected override BTState OnUpdate()
    {
        if (agent.MoveAway(10, blackboard.moveToPosition))
        {
            return BTState.Success;
        }

        return BTState.Running;
    }
}
