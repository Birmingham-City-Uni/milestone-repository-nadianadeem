using UnityEngine;

// Movement for Chibi

public class move : MonoBehaviour {
    // chibi speed
    public float speed = 10.0f;
    // chibi rotation speed
    public float rotationSpeed = 200.0f;
    // Public GameObject to store the seekable object in
    public GameObject seek_me;

    float autoRotSpeed = 0.02f;
    // Bool to turn AutoPilot On/Off
    bool autoPilot = false;

    Vector3 Cross(Vector3 v, Vector3 w) {

        float xMult = v.y * w.z - v.z * w.y;
        float yMult = v.z * w.x - v.x * w.z;
        float zMult = v.x * w.y - v.y * w.x;

        Vector3 crossProd = new Vector3(xMult, yMult, zMult);
        return crossProd;
    }

    float autoSpeed = 0.1f;
    void AutoPilot() {

        // Face the saught object tank
        float angle_to_turn = CalculateAngle();
        this.transform.Rotate(0, angle_to_turn,0);
        // Move the chibi in it's forward direction
        this.transform.Translate(this.transform.forward * autoSpeed, Space.World);
    }

    // Calculate the vector to the seek object
    float CalculateAngle() {

        // Chibi foward facing vector
        Vector3 tF = this.transform.forward;
        // Vector to the seek object
        Vector3 sD = seek_me.transform.position - this.transform.position;

        //float dot;   // store the dot product
        //float angle; // angle var to return

        // Calculate the dot product
        

        // Calculate the angle between the two items - be careful it is in degs and not rads
        float dot = tF.x * sD.x + tF.y * sD.y + tF.z * sD.z;
        float angle = Mathf.Acos(dot / (tF.magnitude * sD.magnitude))  * Mathf.Rad2Deg;

        // Output the angle to the console
        Debug.Log("Angle: " + angle);
        // Output Unitys angle
        Debug.Log("Unity Angle: " + Vector3.Angle(tF, sD));

        // Draw a ray showing the chibi's forward facing vector
        Debug.DrawRay(this.transform.position, tF * 10.0f, Color.green, 2.0f);
        // Draw a ray showing the vector to the saught object
        Debug.DrawRay(this.transform.position, sD, Color.red, 2.0f);

        // Check the z value of the crossproduct and negate the direction if less than 0
        int clockwise = 1;
        Debug.DrawRay(this.transform.position, Cross(tF, sD), Color.blue, 2.0f);
        if (Cross(tF, sD).y < 0.0f)
            clockwise = -1;
        // Use Unity to work out the angle for you
        //float unityAngle = Vector3.SignedAngle(tF, sD, this.transform.forward);
        return clockwise * angle * autoRotSpeed;
    }

    // Calculate the distance from the chibi to whatever it is finding    
    float CalculateDistance() {
        // Chibi position
        Vector3 tP = this.transform.position;
        // Saught object position
        Vector3 sP = seek_me.transform.position;

        // Calculate the distance using pythagoras
        float distance = Mathf.Sqrt(Mathf.Pow(tP.x - sP.x, 2.0f) +
                         Mathf.Pow(tP.y - sP.y, 2.0f) +
                         Mathf.Pow(tP.z - sP.z, 2.0f));

        // Calculate the distance using Unitys vector distance function
        float unityDistance = Vector3.Distance(tP, sP);

        // Print out the two results to the console
        Debug.Log("Distance: " + distance);
        Debug.Log("Unity Distance: " + unityDistance);

        // Return the calculated distance
        return distance;
    }


    void Update() {
        // Get the horizontal and vertical axis.
        // By default they are mapped to the arrow keys.
        // The value is in the range -1 to 1
        float translation = Input.GetAxis("Vertical") * speed;
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;

        if (!autoPilot) {
            // Make it move 10 meters per second instead of 10 meters per frame...
            translation *= Time.deltaTime;
            rotation *= Time.deltaTime;

            // Move translation along the object's z-axis
            transform.Translate(0, 0, translation);

            // Rotate around our y-axis
            transform.Rotate(0, rotation, 0);
        }

        // Check for the spacebar being pressed
        if (Input.GetKeyDown(KeyCode.Space)) {

            // Call CalculateDistance method
            CalculateDistance();
            // Call CalculateAngle method
            CalculateAngle();
        }

        // Check for the T key being pressed
        if (Input.GetKeyDown(KeyCode.T)) {
            // Call CalculateAngle method
            autoPilot = !autoPilot;
        }

        if (autoPilot) {
            // If yes and distance is greater than 5 then execute AutoPilot method
            if (CalculateDistance() > 1.0f) {
                AutoPilot();
            }
            translation *= Time.deltaTime;
            rotation *= Time.deltaTime;
            // Move saught object translation along the object's z-axis
            seek_me.transform.Translate(0, 0, translation);
            // Rotate saught object around our y-axis
            seek_me.transform.Rotate(0, rotation, 0);
        }
    }
}