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
    private Rigidbody rb;
    private float characterRadius;
    public bool calculateOnce;

    [Header("Collision Avoidance Settings")]
    public HashSet<Rigidbody> targets = new HashSet<Rigidbody>();

    [Header("Wall Collision Settings")]
    //Wall Collision variables
    /* How far ahead the ray should extend */
    public float mainWhiskerLen = 1.25f;

    /* The distance away from the collision that we wish go */
    public float wallAvoidDistance = 0.5f;

    public float sideWhiskerLen = 0.701f;

    public float sideWhiskerAngle = 45f;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Sword") && other.GetComponent<Rigidbody>() != null)
        {
            targets.Add(other.GetComponent<Rigidbody>());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Sword"))
        {
            targets.Remove(other.GetComponent<Rigidbody>());
        }
    }

    public static float getBoundingRadius(Transform t)
    {
        SphereCollider col = t.GetComponent<SphereCollider>();
        return Mathf.Max(t.localScale.x, t.localScale.y, t.localScale.z) * col.radius;
    }

    /* Returns the orientation as a unit vector */
    private Vector3 orientationToVector(float orientation)
    {
        return new Vector3(Mathf.Cos(orientation), Mathf.Sin(orientation), 0);
    }

    private bool findObstacle(Vector3[] rayDirs, out RaycastHit firstHit)
    {
        firstHit = new RaycastHit();
        bool foundObs = false;

        for (int i = 0; i < rayDirs.Length; i++)
        {
            float rayDist = (i == 0) ? mainWhiskerLen : sideWhiskerLen;

            RaycastHit hit;

            if (Physics.Raycast(transform.position, rayDirs[i], out hit, rayDist))
            {
                foundObs = true;
                firstHit = hit;
                break;
            }
        }

        return foundObs;
    }

    public void Update()
    {
        if (gridHasChanged)
        {
            pathfindingComponent.UpdatePathfinding(oldWaypoint);
        }
    }

    public bool SeekAndAvoid(float _maxSpeed, Vector3 _waypoint)
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

        /* 1. Find the target that the character will collide with first */

        /* The first collision time */
        float shortestTime = float.PositiveInfinity;

        /* The first target that will collide and other data that
         * we will need and can avoid recalculating */
        Rigidbody firstTarget = null;
        //float firstMinSeparation = 0, firstDistance = 0;
        float firstMinSeparation = 0, firstDistance = 0, firstRadius = 0;
        Vector3 firstRelativePos = Vector3.zero, firstRelativeVel = Vector3.zero;

        //Collsion stuffs
        foreach (Rigidbody r in targets)
        {
            /* Calculate the time to collision */
            Vector3 relativePos = transform.position - r.position;
            Vector3 relativeVel = rb.velocity - r.velocity;
            float distance = relativePos.magnitude;
            float relativeSpeed = relativeVel.magnitude;

            if (relativeSpeed == 0)
            {
                continue;
            }

            float timeToCollision = -1 * Vector3.Dot(relativePos, relativeVel) / (relativeSpeed * relativeSpeed);

            /* Check if they will collide at all */
            Vector3 separation = relativePos + relativeVel * timeToCollision;
            float minSeparation = separation.magnitude;

            float targetRadius = getBoundingRadius(r.transform);

            if (minSeparation > characterRadius + targetRadius)
            //if (minSeparation > 2 * agentRadius)
            {
                continue;
            }

            /* Check if its the shortest */
            if (timeToCollision > 0 && timeToCollision < shortestTime)
            {
                shortestTime = timeToCollision;
                firstTarget = r;
                firstMinSeparation = minSeparation;
                firstDistance = distance;
                firstRelativePos = relativePos;
                firstRelativeVel = relativeVel;
                firstRadius = targetRadius;
            }
        }

        //Wall Collision Stuff
        /* Creates the ray direction vector */
        Vector3[] rayDirs = new Vector3[3];
        rayDirs[0] = transform.forward.normalized;

        float orientation = Mathf.Atan2(rb.velocity.y, rb.velocity.x);

        rayDirs[1] = orientationToVector(orientation + sideWhiskerAngle * Mathf.Deg2Rad);
        rayDirs[2] = orientationToVector(orientation - sideWhiskerAngle * Mathf.Deg2Rad);

        RaycastHit hit;

        //Pathfinding 

        if (pathfindingComponent.path != null)
        {
            if(pathfindingComponent.path.Count > 0)
            {
                Vector3 direction = pathfindingComponent.path[0].worldPosition - transform.position;
                direction.y = 0;

                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), _maxSpeed * Time.deltaTime);

                Vector3 moveVector;

                /* If no collision do nothing */
                if (!findObstacle(rayDirs, out hit))
                {
                    moveVector = direction.normalized * _maxSpeed * Time.deltaTime;
                }

                /* Create a target away from the wall to seek */
                Vector3 targetPostition = hit.point + hit.normal * wallAvoidDistance;

                /* If velocity and the collision normal are parallel then move the target a bit to
                 the left or right of the normal */
                Vector3 cross = Vector3.Cross(rb.velocity, hit.normal);
                if (cross.magnitude < 0.005f)
                {
                    moveVector = transform.position + targetPostition + new Vector3(-hit.normal.y, hit.normal.x, hit.normal.z);
                }

                if (firstTarget == null)
                {
                    moveVector = direction.normalized * _maxSpeed * Time.deltaTime;
                }
                /* If we are going to collide with no separation or if we are already colliding then 
                 * steer based on current position */
                else if (firstMinSeparation <= 0 || firstDistance < characterRadius + firstRadius)
                //if (firstMinSeparation <= 0 || firstDistance < 2 * agentRadius)
                {
                    moveVector = transform.position - firstTarget.position;
                }
                /* Else calculate the future relative position */
                else
                {
                    moveVector = firstRelativePos + firstRelativeVel * shortestTime;
                }

                transform.position += moveVector;

                if (Vector3.Distance(this.transform.position, pathfindingComponent.path[0].worldPosition) < 1f)
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
            finalDest = transform.position + direction.normalized * _maxSpeed;
            SeekAndAvoid(_maxSpeed, finalDest);
            calculateOnce = false;
        }
        else
        {
            SeekAndAvoid(_maxSpeed, finalDest);
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

            SeekAndAvoid(_maxSpeed, finalDest);
            calculateOnce = false;
        }
        else
        {
            SeekAndAvoid(_maxSpeed, finalDest);
        }

        if(pathfindingComponent.path.Count <= 1)
        {
            calculateOnce = true;
            return true;
        }

        return false;
    }

    public bool Wander(float _maxSpeed)
    {
        if (calculateOnce)
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

            finalDest = transform.position + targetPosition.normalized * _maxSpeed;

            SeekAndAvoid(_maxSpeed, finalDest);
            calculateOnce = false;
        }
        else
        {
            SeekAndAvoid(_maxSpeed, finalDest);
        }

        if (pathfindingComponent.path.Count <= 1)
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

        //stuff for the wander behavior
        float theta = Random.value * 2 * Mathf.PI;
        wanderTarget = new Vector3(wanderRadius * Mathf.Cos(theta), wanderRadius * Mathf.Sin(theta), 0f);
        rb = GetComponent<Rigidbody>();
        characterRadius = getBoundingRadius(transform);
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
