using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderNode : ActionNode
{
    public Vector3 lastPosition;
    protected override void OnStart()
    {
        agent.GetComponent<Animator>().SetBool("IsMoving", true);
        agent.calculateOnce = true;
    }

    protected override void OnStop()
    {
        agent.calculateOnce = true;
    }

    protected override BTState OnUpdate()
    {
        lastPosition = agent.transform.position;
        if (agent.Wander(10f))
        {
            if(lastPosition == agent.transform.position)
            {
                agent.Flee(10f, agent.transform.position);
                return BTState.Success;
            }
            if (Vector3.Distance(agent.transform.position, agent.oldWaypoint) < 1f)
            {
                agent.transform.Rotate(0, Random.Range(0, 361), 0);
                return BTState.Success;
            }
        }
        if (Vector3.Distance(agent.transform.position, agent.oldWaypoint) < 1f)
        {
            agent.transform.Rotate(0, Random.Range(0, 361), 0);
            return BTState.Success;
        }

        return BTState.Running;
    }
}
