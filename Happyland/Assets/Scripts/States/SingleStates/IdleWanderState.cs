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
    }

    public override void ReEnter()
    {
    }

    public override void Execute()
    {
        wanderStateManager.Update();

        if (findBoidState.currentReEnteringTime <= 0)
        {
            if ((agent.sensor.Hit == true) && agent.sensor.info.transform.CompareTag("boid"))
            {
                if (scareBoidState.currentReEnteringTime <= 0 && wanderStateManager.GetCurrStateOnStack().GetType() == typeof(WanderFindBoidState))
                {
                    wanderStateManager.PushState(goToBoidState);
                }
            }

            if (Vector3.Distance(agent.transform.position, goToBoidState.dest) < 2f && (wanderStateManager.GetCurrStateOnStack().GetType() == typeof(WanderGoToBoidState)))
            {
                wanderStateManager.PopState();
                wanderStateManager.PushState(scareBoidState);
                scareBoidState.currentReEnteringTime = ReEnteringTime;
            }
        }

        if (scareBoidState.currentReEnteringTime <= 0 && scareBoidState.isComplete)
        {
            wanderStateManager.PopState();
            scareBoidState.isComplete = false;
        }
    }

    public override void Exit()
    {
    }
}
