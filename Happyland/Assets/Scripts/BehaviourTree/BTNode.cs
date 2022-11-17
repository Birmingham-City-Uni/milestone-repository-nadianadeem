using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    [HideInInspector] public Blackboard blackboard;
    [HideInInspector] public Agent agent;
    [TextArea] public string description;

    public BTState Update()
    {
        if(blackboard.stateNameplate == null)
        {
            blackboard.stateNameplate = agent.GetComponentInChildren<TextMeshProUGUI>();
        }

        if (!started)
        {
            
            OnStart();
            started = true;

            if (blackboard.stateNameplate)
            {
                blackboard.stateNameplate.text = this.GetType().ToString().Replace("Node", "");
            }
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
