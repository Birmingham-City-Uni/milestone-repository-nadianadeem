using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    Agent owner;
    public AttackState(Agent owner, StateManager sm): base(owner, sm)
    {
    }

    public override void Enter()
    {
    }

    public override void ReEnter()
    {
    }

    public override void Execute()
    {
        if ((agent.sensor.Hit == true) && agent.sensor.info.collider.gameObject.CompareTag("Player"))
        {
            agent.Arrive(20f, agent.sensor.info.point);
            agent.agentAnimator.SetBool("IsMoving", true);
        }
    }

    public override void Exit()
    {
    }
}
