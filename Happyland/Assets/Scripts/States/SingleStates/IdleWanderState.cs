using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleWanderState : State
{
    public StateManager wanderStateManager;
    public WanderFindBoidState findBoidState;
    public WanderGoToBoidState goToBoidState;
    public WanderScareBoidState scareBoidState;

    public IdleWanderState(Agent owner, StateManager sm) : base(owner, sm)
    {
        wanderStateManager = new StateManager();
        findBoidState = new WanderFindBoidState(owner, sm);
        goToBoidState = new WanderGoToBoidState(owner, sm);
        scareBoidState = new WanderScareBoidState(owner, sm);
        wanderStateManager.Init(findBoidState);
    }

    public override void Enter()
    {
        Debug.Log("Entering Idle");
    }

    public override void ReEnter()
    {
        Debug.Log("Entering Idle");
    }

    public override void Execute()
    {
        Debug.Log("Executing Idle");
        wanderStateManager.Update();
    }

    public override void Exit()
    {
        Debug.Log("Exiting Idle");
    }
}
