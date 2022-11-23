using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderGoToBoidState : State
{
    public Vector3 dest;
    public WanderGoToBoidState(Agent owner, StateManager sm) : base(owner, sm)
    {
    }

    public override void Enter()
    {
        dest = new Vector3(agent.sensor.info.point.x, agent.transform.position.y, agent.sensor.info.point.z);
    }

    public override void ReEnter()
    {
        dest = new Vector3(agent.sensor.info.point.x, agent.transform.position.y, agent.sensor.info.point.z);
        IsReEntering = false;
    }

    public override void Execute()
    {
        if((agent.sensor.Hit == true) && agent.sensor.info.transform.CompareTag("boid"))
        {
            dest = new Vector3(agent.sensor.info.point.x, agent.transform.position.y, agent.sensor.info.point.z);
        }
        agent.SeekAndAvoid(15f, dest);
        agent.agentAnimator.SetBool("IsMoving", true);
    }

    public override void Exit()
    {
    }
}
