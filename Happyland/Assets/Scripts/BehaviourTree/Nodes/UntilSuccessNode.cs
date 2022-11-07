using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UntilSuccessNode : DecoratorNode
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
        if(child.state == BTNode.BTState.Success)
        {
            return BTState.Success;
        }

        return BTState.Running;
    }
}
