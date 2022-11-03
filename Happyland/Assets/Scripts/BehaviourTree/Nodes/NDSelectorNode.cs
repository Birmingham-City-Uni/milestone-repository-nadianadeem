using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NDSelectorNode : CompositeNode
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
        current = Random.Range(0, children.Count);
        var child = children[current];
        switch (child.Update())
        {
            case BTState.Running:
                return BTState.Running;
            case BTState.Failure:
                return BTState.Failure;
            case BTState.Success:
                return BTState.Success;
        }

        return current == children.Count ? BTState.Success : BTState.Running;
    }
}
