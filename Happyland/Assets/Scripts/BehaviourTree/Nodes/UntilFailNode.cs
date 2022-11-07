using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UntilFailNode : DecoratorNode
{
    protected override void OnStart()
    {

    }

    protected override void OnStop()
    {

    }

    protected override BTState OnUpdate()
    {
        child.Update();
        if (child.state == BTNode.BTState.Failure)
        {
            return BTState.Success;
        }

        return BTState.Running;
    }
}
