using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class Agent : MonoBehaviour
{
    [Header("Agent Settings")]
    public StateManager stateManager;
    public Vector3 oldWaypoint;
    public Node lastNode;
    public Animator agentAnimator;
    public sensors sensor;
    public Pathfinding pathfindingComponent;
    public bool gridHasChanged = false;

    [Header("Wander Settings")]
    public float wanderRadius = 1.2f;

    public float wanderDistance = 2f;

    public float wanderJitter = 40f;

    private Vector3 wanderTarget;

    private Vector3 finalDest;
    public bool calculateOnce;

    public void Update()
    {
        if (gridHasChanged)
        {
            pathfindingComponent.UpdatePathfinding(oldWaypoint);
        }
    }

    public bool Seek(float _maxSpeed, Vector3 _waypoint)
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

        if (pathfindingComponent.path != null)
        {
            if(pathfindingComponent.path.Count > 0)
            {
                Vector3 direction = pathfindingComponent.path[0].worldPosition - transform.position;
                direction.y = 0;

                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), _maxSpeed * Time.deltaTime);

                Vector3 moveVector = direction.normalized * _maxSpeed * Time.deltaTime;

                transform.position += moveVector;

                if (Vector3.Distance(this.transform.position, pathfindingComponent.path[0].worldPosition) < 0.2f)
                {
                    pathfindingComponent.path.RemoveAt(0);                  
                }

                return true;
            }
        }

        return false;
    }

    public bool Flee(float _maxSpeed, Vector3 _waypoint)
    {
        if (_waypoint != oldWaypoint && calculateOnce)
        {
            Vector3 direction = transform.position - _waypoint;
            direction.y = 0;
            finalDest = transform.position + direction.normalized * _maxSpeed / 2;
            Seek(_maxSpeed, finalDest);
            calculateOnce = false;
        }
        else
        {
            Seek(_maxSpeed, finalDest);
        }

        if (pathfindingComponent.path.Count <= 1)
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

        if (pathfindingComponent.path.Count > 0 && pathfindingComponent.path != null)
        {
            Vector3 direction = pathfindingComponent.path[0].worldPosition - transform.position;
            direction.y = 0;

            float distance = direction.magnitude;

            float decelerationFactor = distance / 1.3f;

            float speed = _maxSpeed * decelerationFactor;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), _maxSpeed * Time.deltaTime);

            Vector3 moveVector = direction.normalized * Time.deltaTime * speed;
            transform.position += moveVector;

            if (Vector3.Distance(this.transform.position, pathfindingComponent.path[0].worldPosition) < 0.2f)
            {
                pathfindingComponent.path.RemoveAt(0);
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

            Seek(_maxSpeed, finalDest);
            calculateOnce = false;
        }
        else
        {
            Seek(_maxSpeed, finalDest);
        }

        if(pathfindingComponent.path.Count <= 1)
        {
            calculateOnce = true;
            return true;
        }

        return false;
    }

    public void Wander(float _maxSpeed)
    {
        //get the jitter for this time frame
        float jitter = wanderJitter * Time.deltaTime;

        //add a small random vector to the target's position
        wanderTarget += new Vector3(Random.Range(-1f, 1f) * jitter, Random.Range(-1f, 1f) * jitter, 0f);

        //make the wanderTarget fit on the wander circle again
        wanderTarget.Normalize();
        wanderTarget *= wanderRadius;

        //move the target in front of the character
        Vector3 targetPosition = transform.position + transform.right * wanderDistance + wanderTarget;

        Seek(_maxSpeed, targetPosition);
    }

    public void Awake()
    {
        stateManager.spawnState = new SpawnState(this, stateManager);
        stateManager.attackState = new AttackState(this, stateManager);
        stateManager.dieState = new DieState(this, stateManager);

        //stuff for the wander behavior
        float theta = Random.value * 2 * Mathf.PI;
        wanderTarget = new Vector3(wanderRadius * Mathf.Cos(theta), wanderRadius * Mathf.Sin(theta), 0f);
    }

    public void PlaySoundWithDelay(int index, float time)
    {
        StartCoroutine(GameObject.FindGameObjectWithTag("sounds").GetComponent<SceneSound>().PlayDelaySound(index, time));
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
