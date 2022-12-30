using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorNode : CompositeNode
{
    public int current;
    protected override void OnStart()
    {
        current = 0;
    }

    protected override void OnStop()
    {
    }

    protected override BTState OnUpdate()
    {
        var child = children[current];
        switch (child.Update())
        {
            case BTState.Running:
                return BTState.Running;
            case BTState.Failure:
                current++;
                break;
            case BTState.Success:
                return BTState.Success;
        }

        return BTState.Failure;
    }
}
