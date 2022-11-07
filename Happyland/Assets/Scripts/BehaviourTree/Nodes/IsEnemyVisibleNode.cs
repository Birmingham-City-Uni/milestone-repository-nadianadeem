using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsEnemyVisibleNode : ActionNode
{
    protected override void OnStart()
    {
        blackboard.moveToPosition = this.position;
    }

    protected override void OnStop()
    {
    }

    protected override BTState OnUpdate()
    {
        if (agent.sensor.Hit == true && agent.sensor.info.transform.CompareTag("Player"))
        {
            blackboard.moveToPosition = agent.sensor.info.transform.position;
            return BTState.Success;
        }

        return BTState.Running;
    }
}
