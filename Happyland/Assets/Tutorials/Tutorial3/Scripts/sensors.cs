using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sensors : MonoBehaviour
{
    public LayerMask hitMask;
    public enum Type
    {
        Line,
        RayBundle,
        SphereCast,
        BoxCast
    }

    public Type sensorType = Type.Line;
    public float raycastLength = 1.0f;

    [Header("Sphere Settings")]
    public float spherecastRadius = 1.0f;
    Transform cachedTransform;

    // Start is called before the first frame update
    void Start()
    {
        cachedTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        Scan();
    }

    public bool Hit { get; private set; }

    public RaycastHit info = new RaycastHit();

    public bool Scan()
    {
        Hit = false;
        Vector3 dir = cachedTransform.forward;

        switch (sensorType)
        {
            case Type.Line:
                if(Physics.Linecast(cachedTransform.position, cachedTransform.position + dir * raycastLength, out info, hitMask, QueryTriggerInteraction.Ignore))
                {
                    Hit = true;
                    Debug.Log("Hit");
                    return true;
                }
                break;
            case Type.SphereCast:
                if(Physics.SphereCast(new Ray(cachedTransform.position, dir), spherecastRadius, out info, raycastLength, hitMask, QueryTriggerInteraction.Ignore))
                {
                    Hit = true;
                    Debug.Log("Hit");
                    return true;
                }
                break;
        }

        return false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        if(cachedTransform == null)
        {
            cachedTransform = GetComponent<Transform>();
        }
        Scan();

        if (Hit)
        {
            Gizmos.color = Color.red;
        }

        Gizmos.matrix *= Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        float length = raycastLength;

        switch (sensorType)
        {
            case Type.Line:
                if(Hit)
                length = Vector3.Distance(this.transform.position, info.point);
                Gizmos.DrawLine(Vector3.zero, Vector3.forward * length);
                Gizmos.color = Color.green;
                Gizmos.DrawCube(Vector3.forward * length, new Vector3(0.02f, 0.02f, 0.02f));
                break;
            case Type.SphereCast:
                Gizmos.DrawWireSphere(Vector3.zero, spherecastRadius);
                if (Hit)
                {
                    Vector3 ballCenter = info.point + info.normal * spherecastRadius;
                    length = Vector3.Distance(cachedTransform.position, ballCenter);
                }
                Gizmos.DrawLine(Vector3.up * spherecastRadius, Vector3.up * spherecastRadius + Vector3.forward * length);
                Gizmos.DrawLine(-Vector3.up * spherecastRadius, -Vector3.up * spherecastRadius + Vector3.forward * length);
                Gizmos.DrawLine(Vector3.right * spherecastRadius, Vector3.right * spherecastRadius + Vector3.forward * length);
                Gizmos.DrawLine(-Vector3.right * spherecastRadius, -Vector3.right * spherecastRadius + Vector3.forward * length);
                Gizmos.DrawWireSphere(Vector3.forward * length, spherecastRadius);
                break;
        }
    }
}
