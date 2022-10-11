using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    [Header("Agent Settings")]
    public StateManager stateManager;

    public void Move(float _maxSpeed, Vector3 _waypoint)
    {
        Vector3 targetVelocity = _maxSpeed * this.transform.forward * Time.deltaTime;
        this.transform.LookAt(_waypoint);
        this.transform.Translate(targetVelocity, Space.World);
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
