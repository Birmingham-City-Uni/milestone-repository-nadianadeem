using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

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
        if (agent.sensor.Hit == true && agent.sensor.info.transform.gameObject.CompareTag("Player"))
        {
            blackboard.moveToPosition = agent.sensor.info.transform.position;
            blackboard.evadeObject = agent.sensor.info.transform.GetComponent<ThirdPersonController>();
            return BTState.Success;
        }

        return BTState.Running;
    }
}
