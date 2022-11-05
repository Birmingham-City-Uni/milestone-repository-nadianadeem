using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitNode : ActionNode
{
    public float duration = 1;
    float startTime;

    protected override void OnStart()
    {
        startTime = Time.time;
    }

    protected override void OnStop()
    {

    }

    protected override BTState OnUpdate()
    {
        if(Time.time - startTime > duration)
        {
            return BTState.Success;
        }

        return BTState.Running;
    }
}