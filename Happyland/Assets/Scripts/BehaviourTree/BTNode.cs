using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BTNode : ScriptableObject
{
    public enum BTState
    {
        Running,
        Failure,
        Success
    }

    public BTState state = BTState.Running;
    public bool started = false;

    public BTState Update()
    {
        if (!started)
        {
            OnStart();
            started = true;
        }

        state = OnUpdate();

        if(state == BTState.Failure || state == BTState.Success)
        {
            OnStop();
            started = false;
        }

        return state;
    }

    protected abstract void OnStart();
    protected abstract void OnStop();
    protected abstract BTState OnUpdate();

}
