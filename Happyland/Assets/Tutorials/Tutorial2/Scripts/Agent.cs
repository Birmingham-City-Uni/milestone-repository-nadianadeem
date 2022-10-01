using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    StateManager sm = new StateManager();
    bool stateChange = false;

    // Start is called before the first frame update
    void Start()
    {
        sm.ChangeState(new IdleState(this));
    }

    // Update is called once per frame
    void Update()
    {
        sm.Update();
        if(this.transform.position.x > 5.0f && !stateChange)
        {
            sm.ChangeState(new FleeState(this));
            stateChange = !stateChange;
        }
        if (this.transform.position.x < 5.0f && stateChange)
        {
            sm.ChangeState(new FleeState(this));
            stateChange = !stateChange;
        }
    }
}
