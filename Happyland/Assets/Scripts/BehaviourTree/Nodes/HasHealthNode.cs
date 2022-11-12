using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasHealthNode : ActionNode
{
    HealthManager agentHealth;
    protected override void OnStart()
    {
        agentHealth = agent.gameObject.GetComponent<HealthManager>();
    }

    protected override void OnStop()
    {
        
    }

    protected override BTState OnUpdate()
    {
        if(agentHealth.health <= 0)
        {
            return BTState.Failure;
        }

        return BTState.Success;
    }
}
