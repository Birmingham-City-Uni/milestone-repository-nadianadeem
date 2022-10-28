using UnityEngine;

public class IdleWanderState : State
{
    public StateManager wanderStateManager;
    public WanderFindBoidState findBoidState;
    public WanderGoToBoidState goToBoidState;
    public WanderScareBoidState scareBoidState;

    protected bool isBoidFound = false;

    public IdleWanderState(Agent owner, StateManager sm) : base(owner, sm)
    {
        wanderStateManager = new StateManager();
        wanderStateManager.IsStackBased = sm.IsStackBased;
        findBoidState = new WanderFindBoidState(agent, wanderStateManager);
        goToBoidState = new WanderGoToBoidState(agent, wanderStateManager);
        scareBoidState = new WanderScareBoidState(agent, wanderStateManager);
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

        if (findBoidState.currentReEnteringTime <= 0)
        {
            if ((agent.sensor.Hit == true) && agent.sensor.info.transform.tag == "boid" && Vector3.Distance(agent.transform.position, agent.sensor.info.point) < 1f && (wanderStateManager.GetCurrStateOnStack().GetType() == typeof(WanderGoToBoidState)))
            {
                wanderStateManager.PushState(scareBoidState);
            }
            
            if (!isBoidFound && (agent.sensor.Hit == true) && agent.sensor.info.transform.tag == "boid" && (wanderStateManager.GetCurrStateOnStack().GetType() == typeof(WanderFindBoidState)))
            {
                wanderStateManager.PushState(goToBoidState);
                scareBoidState.isComplete = false;
            }
        }


        if (scareBoidState.isComplete)
        {
            wanderStateManager.PopState();
            wanderStateManager.PopState();
        }
    }

    public override void Exit()
    {
        Debug.Log("Exiting Idle");
    }
}
