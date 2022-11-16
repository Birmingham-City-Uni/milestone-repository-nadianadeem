using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackNode : ActionNode
{
    protected override void OnStart()
    {
        agent.GetComponent<Animator>().SetBool("IsMoving", false);
        agent.GetComponent<Animator>().SetTrigger("Attack");
        blackboard.stateNameplate.text = "Attack";
        agent.PlaySoundWithDelay(1, 0.45f);
        
    }

    protected override void OnStop()
    {
        
    }

    protected override BTState OnUpdate()
    {
        return BTState.Success;
    }
}
