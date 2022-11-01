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

    [Header("General Settings")]
    public Type sensorType = Type.Line;
    public float raycastLength = 1.0f;

    [Header("Boxcast Settings")]
    public Vector2 boxExtents = new Vector2(1.0f, 1.0f);

    [Header("Spherecast Settings")]
    public float spherecastRadius = 1.0f;
    Transform cachedTransform;

    [Header("RayBundle Settings")]

    public Transform rayOrigin;

    [Range(1, 40)]
    public int rayResolution = 1;

    [Range(1, 360)]
    public int searchArc = 90;
    private int halfAngle;
    float angleIncrement;

    // Start is called before the first frame update
    void Start()
    {
        cachedTransform = GetComponent<Transform>();
}

    // Update is called once per frame
    void FixedUpdate()
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
                if(Physics.Linecast(this.transform.position, this.transform.position + dir * raycastLength, out info, hitMask, QueryTriggerInteraction.Ignore))
                {
                    Hit = true;
                    return true;
                }
                break;
            case Type.SphereCast:
                if(Physics.SphereCast(new Ray(this.transform.position, dir), spherecastRadius, out info, raycastLength, hitMask, QueryTriggerInteraction.Ignore))
                {
                    Hit = true;
                    return true;
                }
                break;
            case Type.BoxCast:
                if(Physics.CheckBox(this.transform.position, new Vector3(boxExtents.x, boxExtents.y, raycastLength)/2, this.transform.rotation, hitMask, QueryTriggerInteraction.Ignore))
                {
                    Hit = true;
                    return true;
                }
                break;
            case Type.RayBundle:
                halfAngle = searchArc / 2;
                angleIncrement = (float)searchArc / rayResolution;
                float currentAngle = -halfAngle;

                for (int i = 0; i <= rayResolution; i++)
                {
                    if(Physics.Raycast(rayOrigin.transform.position, Quaternion.Euler(new Vector3(0, currentAngle, 0)) * Vector3.forward, out info, raycastLength, hitMask, QueryTriggerInteraction.Ignore))
                    {
                        Hit = true;
                        return true;
                    }
                    currentAngle += angleIncrement;
                }
                break;
        }

        return false;
    }

    void OnDrawGizmos()
    {
        float length = raycastLength;
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

        switch (sensorType)
        {
            case Type.Line:
                length = Vector3.Distance(this.transform.position, info.point);
                Gizmos.DrawLine(Vector3.zero, Vector3.forward * length);
                if (Hit)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawCube(Vector3.forward * length, new Vector3(0.02f, 0.02f, 0.02f));
                }
                break;

            case Type.SphereCast:
                Gizmos.DrawWireSphere(Vector3.zero, spherecastRadius);
                if (Hit)
                {
                    Vector3 hitCenter = info.point + info.normal * spherecastRadius;
                    length = Vector3.Distance(cachedTransform.position, hitCenter);
                }
                else { length = raycastLength; }
                Gizmos.DrawLine(Vector3.up * spherecastRadius, Vector3.up * spherecastRadius + Vector3.forward * length);
                Gizmos.DrawLine(-Vector3.up * spherecastRadius, -Vector3.up * spherecastRadius + Vector3.forward * length);
                Gizmos.DrawLine(Vector3.right * spherecastRadius, Vector3.right * spherecastRadius + Vector3.forward * length);
                Gizmos.DrawLine(-Vector3.right * spherecastRadius, -Vector3.right * spherecastRadius + Vector3.forward * length);
                Gizmos.DrawWireSphere(Vector3.forward * length, spherecastRadius);
                break;

            case Type.BoxCast:
                Gizmos.DrawWireCube(Vector3.zero, new Vector3(boxExtents.x, boxExtents.y, raycastLength));
                break;

            case Type.RayBundle:
                halfAngle = searchArc / 2;
                angleIncrement = (float) searchArc / rayResolution;
                float currentAngle = -halfAngle;

                for(int i = 0; i <= rayResolution; i++)
                {
                    Gizmos.DrawRay( rayOrigin.localPosition, Quaternion.Euler(new Vector3(0, currentAngle, 0)) * Vector3.forward * raycastLength);
                    currentAngle += angleIncrement;
                }
                break;
        }
    }
}
