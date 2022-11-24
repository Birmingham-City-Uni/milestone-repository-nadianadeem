using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieNode : ActionNode
{
    private Animator animator;
    protected override void OnStart()
    {
        animator = agent.GetComponent<Animator>();
        animator.SetTrigger("Die");
        GameObject.FindGameObjectWithTag("WaveSpawner").GetComponent<WaveSpawner>().enemiesLeft--;
    }

    protected override void OnStop()
    {
    }

    protected override BTState OnUpdate()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Die") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            Destroy(agent.gameObject);
            return BTState.Success;
        }

        return BTState.Running;
    }
}
