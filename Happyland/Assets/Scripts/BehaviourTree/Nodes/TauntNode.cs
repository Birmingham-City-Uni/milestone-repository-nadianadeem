using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TauntNode : ActionNode
{
    protected override void OnStart()
    {
        agent.GetComponent<Animator>().SetBool("IsMoving", false);
        agent.GetComponent<Animator>().SetTrigger("Taunt");
    }

    protected override void OnStop()
    {

    }

    protected override BTState OnUpdate()
    {
        return BTState.Success;
    }
}
