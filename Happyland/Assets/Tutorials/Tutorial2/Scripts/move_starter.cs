using UnityEngine;

// Movement for Chibi

public class move_starter : MonoBehaviour{
    // chibi speed
    public float speed = 10.0f;
    // chibi rotation speed
    public float rotationSpeed = 200.0f;
    // Public GameObject to store the seekable object in
    public GameObject seek_me;

    float autoRotSpeed = 0.1f;
    float autoSpeed = 0.1f;

    bool AutoPilotMode = false;

    Vector3 Cross(Vector3 v, Vector3 w)
    {

        Vector3 crossProd = new Vector3( 0, 0, 0);

        crossProd.x = v.y * w.z - v.z * w.y;
        crossProd.y = v.z * w.x - v.x * w.z;
        crossProd.z = v.x * w.y - v.y * w.x;

        return crossProd;
    }

    void AutoPilot()
    {
        float angleToTurn = CalculateAngle();
        this.transform.Rotate(0, angleToTurn, 0);
        this.transform.Translate(this.transform.forward * autoSpeed, Space.World);
    }

    // Calculate the vector to the seek object
    float CalculateAngle()
    {

        // Chibi foward facing vector
        Vector3 tF = this.transform.forward;
        // Vector to the seek object
        Vector3 sD = seek_me.transform.position - this.transform.position;

        float dot = 0.0f;   // store the dot product
        float angle = 0.0f; // angle var to return

        // Calculate the dot product
        dot = (tF.x * sD.x) + (tF.y * sD.y) + (tF.z * sD.z);

        // Calculate the angle between the two items - be careful it is in degs and not rads
        angle = Mathf.Acos(dot / (tF.magnitude * sD.magnitude)) * Mathf.Rad2Deg;

        // Output the angles to the console - these should be the same
        Debug.Log("Angle: " + angle);
        // Output Unitys angle
        Debug.Log("Unity Angle: " + Vector3.Angle(tF, sD));

        // Draw a ray showing the chibi's forward facing vector
        Debug.DrawRay(this.transform.position, tF * 10.0f, Color.green, 2.0f);
        // Draw a ray showing the vector to the saught object
        Debug.DrawRay(this.transform.position, sD, Color.red, 2.0f);
        //Draw a ray of cross product
        Debug.DrawRay(this.transform.position, Cross(tF, sD), Color.blue, 2.0f);

        if (Cross(tF, sD).y < 0.0f)
        {
            angle *= -1;
        }

        return angle * autoRotSpeed;
    }

    // Calculate the distance from the chibi to whatever it is finding
    float CalculateDistance()
    {

        // Chibi position
        Vector3 tP = this.transform.position;
        // Seeking object position
        Vector3 sP = seek_me.transform.position;

        float distance = 0.0f;

        Vector3 goalLength = sP-tP;

        // Calculate the distance between the objects using pythagoras
        distance = Mathf.Sqrt(Mathf.Pow(goalLength.x,2) + Mathf.Pow(goalLength.y, 2) + Mathf.Pow(goalLength.z, 2));

        // Calculate and compare your calculation with the distance using Unitys vector distance function
        float unityDistance = Vector3.Distance(tP, sP);

        // Print out the two results to the console - they should be the same
        Debug.Log("Distance: " + distance);
        Debug.Log("Unity Distance: " + unityDistance);

        return distance;
    }

    void FixedUpdate()
    {
        // Get the horizontal and vertical axis.
        // By default they are mapped to the arrow keys.
        // The value is in the range -1 to 1
        float translation = Input.GetAxis("Vertical") * speed;
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;

        if (!AutoPilotMode)
        {
            // Make it move 10 meters per second instead of 10 meters per frame...
            translation *= Time.deltaTime;
            rotation *= Time.deltaTime;

            // Move translation along the object's z-axis
            transform.Translate(0, 0, translation);

            // Rotate around our y-axis
            transform.Rotate(0, rotation, 0);

        }
        
        // Check for the spacebar being pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {

            // Call CalculateDistance method
            CalculateDistance();
            // Call CalculateAngle method
            CalculateAngle();
        }

        // Check for the T key being pressed
        if (Input.GetKeyDown(KeyCode.T))
        {
            AutoPilotMode = !AutoPilotMode;
        }

        if (AutoPilotMode)
        {
            if (CalculateDistance() > 1.0f)
            {
                AutoPilot();

                // Make it move 10 meters per second instead of 10 meters per frame...
                translation *= Time.deltaTime;
                rotation *= Time.deltaTime;

                // Move translation along the object's z-axis
                seek_me.transform.Translate(0, 0, translation);

                // Rotate around our y-axis
                seek_me.transform.Rotate(0, rotation, 0);
            }
        }
    }
}