using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieNode : ActionNode
{
    protected override void OnStart()
    {
        agent.GetComponent<Animator>().SetTrigger("Die");
    }

    protected override void OnStop()
    {
        Destroy(agent);
    }

    protected override BTState OnUpdate()
    {
        return BTState.Success;
    }
}
