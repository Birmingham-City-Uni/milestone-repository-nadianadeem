using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoesEnemyHaveHighHealthNode : ActionNode
{
    protected override void OnStart()
    {
        
    }

    protected override void OnStop()
    {
    }

    protected override BTState OnUpdate()
    {
        if (agent.sensor.Hit == true && agent.sensor.info.transform.gameObject.CompareTag("Player"))
        {
            HealthManager playerHealthManager = agent.sensor.info.transform.gameObject.GetComponent<HealthManager>();
            if (playerHealthManager.currentHealth <= (playerHealthManager.StartingHealth / 2))
            {
                blackboard.doesPlayerHaveHighHealth = false;
                return BTState.Failure;
            }
            else
            {
                blackboard.doesPlayerHaveHighHealth = true;
                return BTState.Success;
            }
        }

        return BTState.Running;
    }
}
