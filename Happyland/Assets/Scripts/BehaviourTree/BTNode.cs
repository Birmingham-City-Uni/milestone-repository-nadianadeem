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

    [HideInInspector] public BTState state = BTState.Running;
    [HideInInspector] public bool started = false;
    [HideInInspector] public string guid;
    [HideInInspector] public Vector2 position;

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

    public virtual BTNode Clone()
    {
        return Instantiate(this);
    }

    protected abstract void OnStart();
    protected abstract void OnStop();
    protected abstract BTState OnUpdate();

}
