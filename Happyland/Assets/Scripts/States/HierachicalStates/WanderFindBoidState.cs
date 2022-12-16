using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderFindBoidState : State
{
    public float speed = 7.0f;
    private Grid gridComp;
    private int randX, randY;
    public Pathfinding pathfindingComponent;

    public WanderFindBoidState(Agent owner, StateManager sm) : base(owner, sm)
    {
    }

    public override void Enter()
    {
        pathfindingComponent = agent.pathfindingComponent;
        gridComp = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grid>();
        randX = Random.Range(0, gridComp.gridSizeX-1);
        randY = Random.Range(0, gridComp.gridSizeY-1);
        lastLocation = gridComp.grid[randX, randY].worldPosition;
    }

    public override void ReEnter()
    {
        if (lastLocation != null)
        {
            agent.SeekAndAvoid(speed, lastLocation);
            if (Vector3.Distance(agent.transform.position, lastLocation) < 1f)
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
    }
}
