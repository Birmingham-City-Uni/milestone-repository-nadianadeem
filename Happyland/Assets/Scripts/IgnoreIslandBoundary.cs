using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreIslandBoundary : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Collider boundaryCollider = GameObject.FindGameObjectWithTag("boundary").GetComponent<Collider>();
        Physics.IgnoreCollision(boundaryCollider, gameObject.GetComponentInParent<Collider>());
    }
}
