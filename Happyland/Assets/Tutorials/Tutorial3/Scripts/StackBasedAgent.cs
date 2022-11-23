using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackBasedAgent : Agent
{
    public WanderState wander;
    public IdleState idle;
    public SeekState seek;

    sensors sensor;

    // Start is called before the first frame update
    public override void Start()
    {
        wander = new WanderState(this, stateManager);
        idle = new IdleState(this, stateManager);
        seek = new SeekState(this, stateManager);
        stateManager.Init(idle);
        sensor = this.gameObject.GetComponent<sensors>();
    }

    // Update is called once per frame
    public override void FixedUpdate()
    {
        bool foundSeekMe = false;
        stateManager.Update();

        if(wander.currentReEnteringTime <= 0)
        {
            if ((sensor.Hit == true) && (stateManager.GetCurrStateOnStack().GetType() != typeof(SeekState)))
            {
                stateManager.PushState(seek);
                foundSeekMe = true;
            }
            
            if ((sensor.Hit == true) && (stateManager.GetCurrStateOnStack().GetType() == typeof(SeekState)) && foundSeekMe)
            {
                stateManager.PopState();
            }
        }

        if ((sensor.Hit == false) && (stateManager.GetCurrStateOnStack().GetType() != typeof(WanderState)))
        {
            stateManager.PushState(wander);
        }
    }
}
