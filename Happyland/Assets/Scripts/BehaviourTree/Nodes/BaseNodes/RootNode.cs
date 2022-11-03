using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootNode : BTNode
{
    [HideInInspector] public BTNode child;
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override BTState OnUpdate()
    {
        return child.Update();
    }

    public override BTNode Clone()
    {
        RootNode node = Instantiate(this);
        node.child = child.Clone();
        return node;
    }
}
