using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderFindBoidState : State
{
    public float speed = 10.0f;
    private Grid gridComp;
    private int randX, randY;
    public Pathfinding pathfindingComponent;

    public WanderFindBoidState(Agent owner, StateManager sm) : base(owner, sm)
    {
    }

    public override void Enter()
    {
        pathfindingComponent = agent.pathfindingComponent;
        Debug.Log("Entering find boid.");
        gridComp = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grid>();
        randX = Random.Range(0, gridComp.gridSizeX-1);
        randY = Random.Range(0, gridComp.gridSizeY-1);
        lastLocation = gridComp.grid[randX, randY].worldPosition;
    }

    public override void ReEnter()
    {
        Debug.Log("Re-entering find boid.");
        if (lastLocation != null)
        {
            agent.SeekAndAvoid(speed, lastLocation);
            if (Vector3.Distance(agent.transform.position, lastLocation) < 0.5f)
            {
                IsReEntering = false;
                randX = Random.Range(0, gridComp.gridSizeX - 1);
                randY = Random.Range(0, gridComp.gridSizeY - 1);
                lastLocation = gridComp.grid[randX, randY].worldPosition;
            }
        }
    }

    public override void Execute()
    {
        Debug.Log("Executing find boid.");
        
        if(gridComp.grid[randX, randY].worldPosition != null && pathfindingComponent != null)
        {
            if (agent.SeekAndAvoid(speed, gridComp.grid[randX, randY].worldPosition))
            {
                agent.agentAnimator.SetBool("IsMoving", true);
                if (pathfindingComponent.path.Count <  1)
                {
                    randX = Random.Range(0, gridComp.gridSizeX - 1);
                    randY = Random.Range(0, gridComp.gridSizeY - 1);
                    lastLocation = gridComp.grid[randX, randY].worldPosition;
                }
            }
            else
            {
                randX = Random.Range(0, gridComp.gridSizeX - 1);
                randY = Random.Range(0, gridComp.gridSizeY - 1);
                lastLocation = gridComp.grid[randX, randY].worldPosition;
            }
        }
    }

    public override void Exit()
    {
        Debug.Log("Exiting find boid.");
    }
}
