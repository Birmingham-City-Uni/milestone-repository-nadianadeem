using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieState : State
{
    bool isBombSpawned = false;
    GameObject bomb;
    public DieState(Agent owner, StateManager sm) : base(owner, sm)
    {
    }

    public override void Enter()
    {
        Debug.Log("Entering Die");
        agent.agentAnimator.SetBool("IsMoving", false);
    }

    public override void ReEnter()
    {
        Debug.Log("Entering Die");
    }

    public override void Execute()
    {
        Debug.Log("Executing Die");
        agent.agentAnimator.SetTrigger("Die");

        if (!isBombSpawned && agent is MiniBomb)
        {
            if (agent.agentAnimator.GetCurrentAnimatorStateInfo(0).IsName("Explode"))
            {
                bomb = GameObject.Instantiate(agent.gameObject.GetComponent<ExplosionHolder>().smallExplosionPrefab, agent.sensor.rayOrigin.transform.position, agent.transform.rotation);
                isBombSpawned = true;
                GameObject.FindGameObjectWithTag("sounds").GetComponent<SceneSound>().PlaySound(0);
                GameObject.Destroy(agent.gameObject);
            }
        }
    }

    public override void Exit()
    {
        Debug.Log("Exiting Die");
    }
}
