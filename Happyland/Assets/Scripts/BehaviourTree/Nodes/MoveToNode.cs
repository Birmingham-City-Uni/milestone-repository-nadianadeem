using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToNode : ActionNode
{
    protected override void OnStart()
    {
        agent.GetComponent<Animator>().SetBool("IsMoving", true);
    }

    protected override void OnStop()
    {
        agent.GetComponent<Animator>().SetBool("IsMoving", false);
    }

    protected override BTState OnUpdate()
    {
        if (agent.SeekAndAvoid(10, blackboard.moveToPosition))
        {
            if (agent.pathfindingComponent.path.Count < 1)
            {
                return BTState.Success;
            }
        }
        if (agent.pathfindingComponent.path.Count < 1)
        {
            return BTState.Success;
        }

        return BTState.Running;
    }
}
