using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBomb : Agent
{
    public SpawnState spawn;
    public AttackState attack;
    public IdleWanderState idle;
    public DieState die;

    sensors sensor;
    bool isExplosionReady = false;

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

        if (idle.currentReEnteringTime <= 0)
        {
            if ((sensor.Hit == true) && (stateManager.GetCurrStateOnStack().GetType() != typeof(AttackState)))
            {
                stateManager.PushState(attack);
            }

            if (isExplosionReady)
            {
                stateManager.PushState(die);
            }
        }

        if ((sensor.Hit == false) && (stateManager.GetCurrStateOnStack().GetType() != typeof(IdleWanderState)))
        {
            stateManager.PushState(idle);
        }
    }
}
