using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BehaviourTree : ScriptableObject
{
    public BTNode rootNode;
    public BTNode.BTState treeState = BTNode.BTState.Running;

    public BTNode.BTState Update()
    {
        return rootNode.Update();
    }
}
