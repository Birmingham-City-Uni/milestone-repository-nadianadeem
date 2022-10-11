using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderState : State
{
    float speed = 10.0f;
    Vector3 wp;
    float range = 20.0f;
    GameObject waypoint;

    public WanderState(Agent owner, StateManager sm): base(owner, sm)
    {

    }

    public override void Enter()
    {
        Debug.Log("Entering wanter state.");
        waypoint = GameObject.Find("waypoint");
        Wander();
    }

    public override void Execute()
    {
        Debug.Log("Updating wander state");
        agent.Move(speed, wp);
        if((agent.transform.position - wp).magnitude < 0.1f)
        {
            Wander();
        }
    }

    public override void Exit()
    {
        Debug.Log("exiting wanter state.");
    }

    void Wander()
    {
        wp = new Vector3(Random.Range(-range, range), agent.transform.position.y, Random.Range(-range, range));
        waypoint.transform.position = wp;
    }
}
