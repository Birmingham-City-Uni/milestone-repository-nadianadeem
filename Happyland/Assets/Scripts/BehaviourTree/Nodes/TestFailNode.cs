using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFailNode : ActionNode
{
    protected override void OnStart()
    {
        
    }

    protected override void OnStop()
    {
        
    }

    protected override BTState OnUpdate()
    {
        return BTState.Failure;
    }
}
