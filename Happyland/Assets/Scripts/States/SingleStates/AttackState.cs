using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    public Vector3 dest;
    public AttackState(Agent owner, StateManager sm) : base(owner, sm)
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
        if ((agent.sensor.Hit == true) && agent.sensor.info.transform.CompareTag("Player"))
        {
            dest = new Vector3(agent.sensor.info.point.x, agent.transform.position.y, agent.sensor.info.point.z);
        }
        agent.Arrive(15f, dest);
        agent.agentAnimator.SetBool("IsMoving", true);
    }

    public override void Exit()
    {
    }
}
