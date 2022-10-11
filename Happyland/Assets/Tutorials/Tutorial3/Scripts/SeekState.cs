using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekState : State
{
    Agent owner;
    float seekFor = 0.1f;

    public SeekState(Agent owner, StateManager sm): base(owner, sm)
    {
    }

    public override void Enter()
    {
        Debug.Log("Entering seek state.");
    }

    public override void Execute()
    {
        Debug.Log("Updating seek state.");
        seekFor -= Time.deltaTime;
        if(seekFor <= 0.0f)
        {
            sm.PopState();
        }
    }

    public override void Exit()
    {
        Debug.Log("Exiting seek state.");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
