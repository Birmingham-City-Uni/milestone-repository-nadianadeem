using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToNode : ActionNode
{
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
        
    }

    protected override BTState OnUpdate()
    {
        if (agent.Move(10, blackboard.moveToPosition))
        {
            if (Vector3.Distance(agent.transform.position, blackboard.moveToPosition) < 1f)
            {
                return BTState.Success;
            }
        }

        return BTState.Running;
    }
}
