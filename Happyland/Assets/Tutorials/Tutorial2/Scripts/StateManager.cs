using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager
{
    State currState;
    public void ChangeState(State newState)
    {
        if (currState != null)
        {
            currState.Exit();
        }
        currState = newState;
        newState.Enter();
    }

    // Update is called once per frame
    public void Update()
    {
        if (currState != null)
        {
            currState.Execute();
        }
    }
}
