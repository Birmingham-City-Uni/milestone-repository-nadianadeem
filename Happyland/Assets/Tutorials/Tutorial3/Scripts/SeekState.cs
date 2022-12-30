using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekState : State
{
    Agent owner;
    float seekFor = 1.0f;

    public SeekState(Agent owner, StateManager sm): base(owner, sm)
    {
    }

    public override void Enter()
    {
    }

    public override void ReEnter()
    {
    }

    public override void Execute()
    {
        seekFor -= Time.deltaTime;
        if(seekFor <= 0.0f)
        {
            sm.PopState();
        }
    }

    public override void Exit()
    {
    }
}
