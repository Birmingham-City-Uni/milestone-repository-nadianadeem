using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsEnemyVisibleNode : ActionNode
{
    public sensors sensor;

    protected override void OnStart()
    { 
    }

    protected override void OnStop()
    {
    }

    protected override BTState OnUpdate()
    {
        if (sensor.Hit == true && sensor.info.transform.CompareTag("Player"))
        {
            return BTState.Success;
        }

        return BTState.Running;
    }
}
