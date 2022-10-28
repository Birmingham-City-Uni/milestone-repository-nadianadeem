using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBomb : Agent
{
    public SpawnState spawn;
    public AttackState attack;
    public IdleWanderState idle;
    public DieState die;

    private bool isReadyToDie = false;

    // Start is called before the first frame update
    public override void Start()
    {
        spawn = new SpawnState(this, stateManager);
        attack = new AttackState(this, stateManager);
        idle = new IdleWanderState(this, stateManager);
        die = new DieState(this, stateManager);
        stateManager.Init(spawn);
        sensor = this.gameObject.GetComponent<sensors>();
    }

    // Update is called once per frame
    public override void FixedUpdate()
    {
        stateManager.Update();

        if ((sensor.Hit == true) && sensor.info.transform.tag == "Player" && (stateManager.GetCurrStateOnStack().GetType() == typeof(IdleWanderState)))
        {
            stateManager.PushState(attack);
        }

        if (!isReadyToDie && Vector3.Distance(this.transform.position, sensor.info.point) < 1f && (stateManager.GetCurrStateOnStack().GetType() == typeof(AttackState)))
        {
            stateManager.PushState(die);
            isReadyToDie = true;
        }

        if (stateManager.GetCurrStateOnStack().GetType() == typeof(SpawnState))
        {
            stateManager.PushState(idle);
        }
    }
}
