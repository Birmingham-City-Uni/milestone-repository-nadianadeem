using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

/// <summary>
/// A hummingbird ML agent
/// </summary>

public class HummingbirdAgent : Unity.MLAgents.Agent
{
    [Tooltip("Force to apply when moving")]
    public float moveForce = 3f;

    [Tooltip("Speed to pitch up or down")]
    public float pitchSpeed = 200f;

    [Tooltip("Speed to rotate around the up axis")]
    public float yawSpeed = 200f;

    [Tooltip("Transform at the tip pf the beak")]
    public Transform beakTip;

    [Tooltip("Whether this is training or gameplay mode")]
    public bool trainingMode;

    //rb of agent
    new private Rigidbody rigidbody;

    //Flower area the agent is in.
    private FlowerArea flowerArea;

    //Nearest flower to the agent.
    private Flower nearestFlower;

    //Allows for smoother pitch changes.
    private float smoothPitchChange = 0f;

    //Allows for smoother yaw changes.
    private float smoothYawChange = 0f;

    //Max angle the bird can pitch up or down.
    private const float maxPitchAngle = 80f;

    //Max distance from the beak tip to accept nectar collision.
    private const float beakTipRadius = 0.008f;

    //Whether agent is frozen (intentionally not flying).
    private bool frozen = false;

    /// <summary>
    /// The amount of nectar obtained this episode.
    /// </summary>
    public float NectarObtained { get; private set; }

    public override void Initialize()
    {
        rigidbody = GetComponent<Rigidbody>();
        flowerArea = GetComponentInParent<FlowerArea>();

        //If not training mode no max set. Play forever.
        if (!trainingMode)
        {
            MaxStep = 0;
        }
    }

    public override void OnEpisodeBegin()
    {
        if (trainingMode)
        {
            //Only reset flowers in training when there is one agent per area.
            flowerArea.ResetFlowers();
        }

        //Reset nectar
        NectarObtained = 0;

        //Zero velocities.
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;

        // Default to spawning infront of flower.
        bool inFrontOfFlower = true;
        if (trainingMode)
        {
            //Spawn in front of flower 50% of the time.
            inFrontOfFlower = UnityEngine.Random.value > 0.5f;
        }

        //Move agent to random position.
        MoveToSafeRandomPosition(inFrontOfFlower);

        //Recalculate the nearest flower after agent has moved.
        UpdateNearestFlower();
    }

    /// <summary>
    /// Called when an action is received from neural network.
    /// 
    /// vectorAction[i] represents:
    /// Index 0: move vector x (+1 = right, -1 = left)
    /// Index 1: move vector y (+1 = up, -1 = down)
    /// Index 2: mvoe vector z (+1 = forward, -1 = backward)
    /// Index 3: pitch angle (+1 = pitch up, -1 = pitch down)
    /// Index 4: yaw angle (+1 = turn right, -1 = turn left)
    /// </summary>
    /// <param name="actions"> The actions to take. </param>
    public override void OnActionReceived(ActionBuffers actions)
    {
        if (frozen) return;

        Vector3 move = new Vector3(actions.ContinuousActions[0], actions.ContinuousActions[1], actions.ContinuousActions[2]);

        //Add force in direction of move vector.
        rigidbody.AddForce(move * moveForce);

        //Get current rotation
        Vector3 rotationVector = transform.rotation.eulerAngles;

        //Calculate pitch and yaw rotation.
        float pitchChange = actions.ContinuousActions[3];
        float yawChange = actions.ContinuousActions[4];

        //Smooth rotation
        smoothPitchChange = Mathf.MoveTowards(smoothPitchChange, pitchChange, 2f * Time.fixedDeltaTime);
        smoothYawChange = Mathf.MoveTowards(smoothYawChange, yawChange, 2f * Time.fixedDeltaTime);

        //Calculate new pitch and yaw and clamp to stop flip.
        float pitch = rotationVector.x + smoothPitchChange * Time.fixedDeltaTime * pitchSpeed;
        if (pitch > 180f)
        {
            pitch -= 360f;
        }
        pitch = Mathf.Clamp(pitch, -maxPitchAngle, maxPitchAngle);

        float yaw = rotationVector.y + smoothYawChange * Time.fixedDeltaTime * yawSpeed;

        transform.rotation = Quaternion.Euler(pitch, yaw, 0);
    }

    /// <summary>
    /// Collect vector observations from the environment.
    /// </summary>
    /// <param name="sensor">The vector sensor.</param>
    public override void CollectObservations(VectorSensor sensor)
    {
        if(nearestFlower == null)
        {
            sensor.AddObservation(new float[10]);
            return;
        }

        //Observes local rotation (4 obvs).
        sensor.AddObservation(transform.localRotation.normalized);

        //Get a vector to the nearest flower.
        Vector3 toFlower = nearestFlower.FlowerCenterPosition - beakTip.position;
        //Observe a normalised vector to the nearest flower (3 obvs).
        sensor.AddObservation(toFlower.normalized);

        //Observe dot prod that indicates whether the beak tip is infront of the flower (1 obvs).
        // (+1 beak tip is directly infront, -1 directly behind)
        sensor.AddObservation(Vector3.Dot(toFlower.normalized, -nearestFlower.FlowerUpVector.normalized));

        //Observe dot product that indicates if the beak is pointing toward the flower (1 obvs).
        // (+1 means that the beak is pointing firectly at the flower, -1 means directly away)
        sensor.AddObservation(Vector3.Dot(beakTip.forward.normalized, -nearestFlower.FlowerUpVector.normalized));

        //Observe the relative distance fromt he beak tip to the flower (1 obvs).
        sensor.AddObservation(toFlower.magnitude / FlowerArea.areaDiameter);

        //10 total obvs.
    }

    /// <summary>
    /// When behaviour type is set to "Heuristic Only" this function will be called. 
    /// Its return values will be fed into 
    /// <see cref="OnActionReceived(ActionBuffers)"/> Instead of using the neural network.
    /// </summary>
    /// <param name="actionsOut"> And output action array.</param>
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        //Create placeholders for all movement.
        Vector3 forward = Vector3.zero;
        Vector3 left = Vector3.zero;
        Vector3 up = Vector3.zero;
        float pitch = 0f;
        float yaw = 0f;

        //Convert keyboard into movement and turning. All values inbetween -1 and +1;

        //Forward/Backward
        if (Input.GetKey(KeyCode.W)) forward = transform.forward;
        else if (Input.GetKey(KeyCode.S)) forward = -transform.forward;

        //Left/Right
        if (Input.GetKey(KeyCode.A)) left = -transform.right;
        else if (Input.GetKey(KeyCode.D)) left = transform.right;

        //Up/Down
        if (Input.GetKey(KeyCode.E)) up = transform.up;
        else if (Input.GetKey(KeyCode.C)) up = -transform.up;

        //Pitch up/down
        if (Input.GetKey(KeyCode.UpArrow)) pitch = 1f;
        else if (Input.GetKey(KeyCode.DownArrow)) pitch = -1f;

        //Turn left right
        if (Input.GetKey(KeyCode.LeftArrow)) yaw = 1f;
        else if (Input.GetKey(KeyCode.RightArrow)) yaw = -1f;

        //Combine
        Vector3 combined = (forward + left + up).normalized;

        //Add 3 movement values.
        var continuousActionsOut = actionsOut.ContinuousActions;

        continuousActionsOut[0] = combined.x;
        continuousActionsOut[1] = combined.y;
        continuousActionsOut[2] = combined.z;
        continuousActionsOut[3] = pitch;
        continuousActionsOut[4] = yaw;
    }

    //Prevent agent moving.
    public void FreezeAgent()
    {
        Debug.Assert(trainingMode == false, "Freeze/Unfreeze not supported in training");
        frozen = true;
        rigidbody.Sleep();
    }

    //Resume agent movement.
    public void UnfreezeAgent()
    {
        Debug.Assert(trainingMode == false, "Freeze/Unfreeze not supported in training");
        frozen = false;
        rigidbody.WakeUp();
    }

    /// <summary>
    /// Update nearest flower to the agent.
    /// </summary>
    private void UpdateNearestFlower()
    {
        foreach(Flower flower in flowerArea.flowers)
        {
            if(nearestFlower == null && flower.HasNectar)
            {
                nearestFlower = flower;
            }
            else if (flower.HasNectar)
            {
                //Calculate distance to this flower and distance to current nearest flower.
                float distancetoFlower = Vector3.Distance(flower.transform.position, beakTip.position);
                float distanceToCurrentNearestFlower = Vector3.Distance(nearestFlower.transform.position, beakTip.position);

                //If current nearest flower is empty or this flower is closer update nearest flower.
                if(!nearestFlower.HasNectar || distancetoFlower < distanceToCurrentNearestFlower)
                {
                    nearestFlower = flower;
                }
            }
        }
    }

    /// <summary>
    /// Move agent to a safe random position.
    /// If infront of flower point beak at flower.
    /// </summary>
    /// <param name="inFrontOfFlower"> Wherther to choose spot infront of the flower.</param>
    private void MoveToSafeRandomPosition(bool inFrontOfFlower)
    {
        bool safePositionFound = false;
        int attemptsRemaining = 100; // Prevent an infinite loop
        Vector3 potentialPosition = Vector3.zero;
        Quaternion potentialRotation = new Quaternion();

        // Loop until a safe position is found or we run out of attempts
        while (!safePositionFound && attemptsRemaining > 0)
        {
            attemptsRemaining--;
            if (inFrontOfFlower)
            {
                // Pick a random flower
                Flower randomFlower = flowerArea.flowers[UnityEngine.Random.Range(0, flowerArea.flowers.Count)];

                float distanceFromFlower = UnityEngine.Random.Range(.1f, .2f);
                potentialPosition = randomFlower.transform.position + randomFlower.FlowerUpVector * distanceFromFlower;

                Vector3 toFlower = randomFlower.FlowerCenterPosition - potentialPosition;
                potentialRotation = Quaternion.LookRotation(toFlower, Vector3.up);
            }
            else
            {
                // Pick a random height from the ground
                float height = UnityEngine.Random.Range(1.2f, 2.5f);

                // Pick a random radius from the center of the area
                float radius = UnityEngine.Random.Range(2f, 7f);

                // Pick a random direction rotated around the y axis
                Quaternion direction = Quaternion.Euler(0f, UnityEngine.Random.Range(-180f, 180f), 0f);

                // Combine height, radius, and direction to pick a potential position
                potentialPosition = flowerArea.transform.position + Vector3.up * height + direction * Vector3.forward * radius;

                // Choose and set random starting pitch and yaw
                float pitch = UnityEngine.Random.Range(-60f, 60f);
                float yaw = UnityEngine.Random.Range(-180f, 180f);
                potentialRotation = Quaternion.Euler(pitch, yaw, 0f);
            }

            // Check to see if the agent will collide with anything
            Collider[] colliders = Physics.OverlapSphere(potentialPosition, 0.05f);

            // Safe position has been found if no colliders are overlapped
            safePositionFound = colliders.Length == 0;
        }

        Debug.Assert(safePositionFound, "Could not find a safe position to spawn");

        // Set the position and rotation
        transform.position = potentialPosition;
        transform.rotation = potentialRotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        TriggerEnterOrStay(other);        
    }

    private void OnTriggerStay(Collider other)
    {
        TriggerEnterOrStay(other);
    }

    private void TriggerEnterOrStay(Collider collider)
    {
        //Check if agent is colliding with nectar.
        if (collider.CompareTag("nectar"))
        {
            Vector3 closestPointToTheBeakTip = collider.ClosestPoint(beakTip.position);
            if(Vector3.Distance(beakTip.position, closestPointToTheBeakTip) < beakTipRadius)
            {
                // Look up the flower for this nectar collider.
                Flower flower = flowerArea.GetFlowerFromNectar(collider);

                //Attempt to take 0.01 nectar. 
                float nectarReceived = flower.Feed(.01f);

                //Keep track of nectar obtained.
                NectarObtained += nectarReceived;

                if (trainingMode)
                {
                    //Calculate reward for getting nectar.
                    float bonus = 0.02f * Mathf.Clamp01(Vector3.Dot(transform.forward.normalized, -nearestFlower.FlowerUpVector.normalized));
                    AddReward(.01f + bonus);
                }

                //If flower is empty update nearest flower.
                if (!flower.HasNectar)
                {
                    UpdateNearestFlower();
                }
            }
        }
    }

    /// <summary>
    /// Called when the agent collides with something solid.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if(trainingMode && collision.collider.CompareTag("boundary"))
        {
            //Collided with the area boundary, give a negative reward.
            AddReward(-.5f);
        }
    }

    private void Update()
    {
        //Draw line from beak to flower.
        if(nearestFlower != null)
        {
            Debug.DrawLine(beakTip.position, nearestFlower.FlowerCenterPosition, Color.green);
        }
    }

    private void FixedUpdate()
    {
        if(nearestFlower != null && !nearestFlower.HasNectar)
        {
            UpdateNearestFlower();
        }
    }
}
