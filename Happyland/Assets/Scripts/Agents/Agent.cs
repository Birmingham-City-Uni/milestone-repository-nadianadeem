using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    [Header("Agent Settings")]
    public StateManager stateManager;

    public Pathfinding pathfindingComponent;

    public bool Move(float _maxSpeed, Vector3 _waypoint)
    {
        pathfindingComponent.UpdatePathfinding(_waypoint);
        Vector3 targetVelocity = _maxSpeed * this.transform.forward * Time.deltaTime;
        if(pathfindingComponent.grid.path.Count > 0)
        {
            this.transform.LookAt(pathfindingComponent.grid.path[0].worldPosition);
            this.transform.Translate(targetVelocity, Space.World);
            return true;
        }

        return false;
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        
    }

    // Update is called once per frame
    public virtual void FixedUpdate()
    {

    }
}
