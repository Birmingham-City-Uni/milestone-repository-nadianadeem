using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NDSelectorNode : CompositeNode
{
    public int current;
    protected override void OnStart()
    {
        current = Random.Range(0, children.Count);
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
                break;
            case BTState.Failure:
                return BTState.Failure;
            case BTState.Success:
                return BTState.Success;
        }

        return BTState.Running;
    }
}
