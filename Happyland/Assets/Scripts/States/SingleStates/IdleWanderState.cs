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
            if ((agent.sensor.Hit == true) && agent.sensor.info.transform.CompareTag("boid"))
            {
                if (Vector3.Distance(agent.transform.position, agent.sensor.info.point) < 1f && (wanderStateManager.GetCurrStateOnStack().GetType() == typeof(WanderGoToBoidState)))
                {
                    wanderStateManager.PopState();
                    wanderStateManager.PushState(scareBoidState);
                    scareBoidState.currentReEnteringTime = ReEnteringTime;
                }

                if (scareBoidState.currentReEnteringTime <= 0 && wanderStateManager.GetCurrStateOnStack().GetType() == typeof(WanderFindBoidState))
                {
                    wanderStateManager.PushState(goToBoidState);
                    scareBoidState.isComplete = false;
                    scareBoidState.currentReEnteringTime = ReEnteringTime;
                }
            }
        }

        if (scareBoidState.currentReEnteringTime <= 0 && scareBoidState.isComplete)
        {
            wanderStateManager.PopState();
        }
    }

    public override void Exit()
    {
        Debug.Log("Exiting Idle");
    }
}
