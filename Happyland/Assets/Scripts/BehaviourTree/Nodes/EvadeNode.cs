using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvadeNode : ActionNode
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
        agentAnimator.SetBool("IsMoving", false);
    }

    protected override BTState OnUpdate()
    {
        if (agent.Evade(10, blackboard.moveToPosition, blackboard.evadeObject))
        {
            return BTState.Success;
        }

        return BTState.Running;
    }
}
