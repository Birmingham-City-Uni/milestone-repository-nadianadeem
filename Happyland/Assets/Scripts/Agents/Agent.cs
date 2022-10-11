using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    StateManager sm = new StateManager();
    bool stateChange = false;

    // Start is called before the first frame update
    public virtual void Start()
    {
        sm.ChangeState(new SpawnState(this, sm));
    }

    // Update is called once per frame
    public virtual void Update()
    {
        sm.Update();
        if (this.transform.position.x > 5.0f && !stateChange)
        {
            sm.ChangeState(new EvadeState(this, sm));
            stateChange = !stateChange;
        }
        if (this.transform.position.x < 5.0f && stateChange)
        {
            sm.ChangeState(new AttackState(this, sm));
            stateChange = !stateChange;
        }
    }
}
