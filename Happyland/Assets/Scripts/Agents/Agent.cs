using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class Agent : MonoBehaviour
{
    [Header("Agent Settings")]
    public StateManager stateManager;
    public Vector3 oldWaypoint;
    public Animator agentAnimator;
    public sensors sensor;
    public Pathfinding pathfindingComponent;

    private Vector3 finalDest;
    public bool calculateOnce;

    public bool MoveTo(float _maxSpeed, Vector3 _waypoint)
    {
        if(_waypoint != oldWaypoint)
        {
            oldWaypoint = _waypoint;
            pathfindingComponent.UpdatePathfinding(_waypoint);
        }
        
        if (pathfindingComponent.grid.NodeFromWorldPoint(this.transform.position).isWater)
        {
            _maxSpeed = _maxSpeed / 2;
        }

        Vector3 targetVelocity = _maxSpeed * this.transform.forward * Time.deltaTime;

        if (pathfindingComponent.grid.path != null)
        {
            if(pathfindingComponent.grid.path.Count > 0)
            {
                Vector3 direction = pathfindingComponent.grid.path[0].worldPosition - transform.position;
                direction.y = 0;

                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), _maxSpeed * Time.deltaTime);

                Vector3 moveVector = direction.normalized * _maxSpeed * Time.deltaTime;

                transform.position += moveVector;

                if (Vector3.Distance(this.transform.position, pathfindingComponent.grid.path[0].worldPosition) < 0.2f)
                {
                    pathfindingComponent.grid.path.RemoveAt(0);
                }

                return true;
            }
        }

        return false;
    }

    public bool MoveAway(float _maxSpeed, Vector3 _waypoint)
    {
        if (_waypoint != oldWaypoint && calculateOnce)
        {
            Vector3 direction = transform.position - _waypoint;
            direction.y = 0;
            finalDest = transform.position + direction.normalized * _maxSpeed / 2;
            MoveTo(_maxSpeed, finalDest);
            calculateOnce = false;
        }
        else
        {
            MoveTo(_maxSpeed, finalDest);
        }

        if (pathfindingComponent.grid.path.Count <= 1)
        {
            calculateOnce = true;
            return true;
        }

        return false;
    }

    public bool Arrive(float _maxSpeed, Vector3 _waypoint)
    {
        if (_waypoint != oldWaypoint)
        {
            oldWaypoint = _waypoint;
            pathfindingComponent.UpdatePathfinding(_waypoint);
        }

        if (pathfindingComponent.grid.NodeFromWorldPoint(this.transform.position).isWater)
        {
            _maxSpeed = _maxSpeed / 2;
        }

        Vector3 targetVelocity = _maxSpeed * this.transform.forward * Time.deltaTime;

        if (pathfindingComponent.grid.path.Count > 0 && pathfindingComponent.grid.path != null)
        {
            Vector3 direction = pathfindingComponent.grid.path[0].worldPosition - transform.position;
            direction.y = 0;

            float distance = direction.magnitude;

            float decelerationFactor = distance / 1.3f;

            float speed = _maxSpeed * decelerationFactor;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), _maxSpeed * Time.deltaTime);

            Vector3 moveVector = direction.normalized * Time.deltaTime * speed;
            transform.position += moveVector;

            if (Vector3.Distance(this.transform.position, pathfindingComponent.grid.path[0].worldPosition) < 0.2f)
            {
                pathfindingComponent.grid.path.RemoveAt(0);
            }

            return true;
        }

        return false;
    }

    public bool Evade(float _maxSpeed, Vector3 _waypoint, ThirdPersonController target)
    {
        if (_waypoint != oldWaypoint && calculateOnce)
        {
            int iterationAhead = 5;

            Vector3 targetSpeed = target.velocity;

            Vector3 targetFuturePosition = _waypoint + (targetSpeed * iterationAhead);

            Vector3 direction = transform.position - targetFuturePosition;
            direction.y = 0;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), _maxSpeed * Time.deltaTime);

            finalDest = transform.position - direction.normalized * _maxSpeed;

            MoveTo(_maxSpeed, finalDest);
            calculateOnce = false;
        }
        else
        {
            MoveTo(_maxSpeed, finalDest);
        }

        if(pathfindingComponent.grid.path.Count <= 1)
        {
            calculateOnce = true;
            return true;
        }


        return false;
        
    }

    public void Awake()
    {
        stateManager.spawnState = new SpawnState(this, stateManager);
        stateManager.attackState = new AttackState(this, stateManager);
        stateManager.dieState = new DieState(this, stateManager);
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
