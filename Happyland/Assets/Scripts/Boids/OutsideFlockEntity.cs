using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutsideFlockEntity : MonoBehaviour
{
    public float speed = 0.5f;
    Vector3 globalOrigin;
    float rotSpeed = 2f;
    float neighbourDistance = 3f;
    bool turning = true;

    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(0.5f, 2f);
        globalOrigin = GameObject.FindGameObjectWithTag("OutsideGrid").transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.position.x >= (globalOrigin.x + OutsideGridFlock.airSizeX / 2) || transform.position.x <= (globalOrigin.x + -OutsideGridFlock.airSizeX / 2))
        {
            turning = true;
        }
        else if (transform.position.z >= (globalOrigin.z + OutsideGridFlock.airSizeY / 2) || transform.position.z <= (globalOrigin.z + -OutsideGridFlock.airSizeY / 2))
        {
            turning = true;
        }
        else if (transform.position.y <= OutsideGridFlock.minFlockHeight)
        {
            turning = true;
        }
        else
        {
            turning = false;
        }

        if (turning)
        {
            Vector3 dir = (globalOrigin + new Vector3(0, Random.Range(OutsideGridFlock.minFlockHeight, OutsideGridFlock.minFlockHeight + 1f)) - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), rotSpeed * Time.deltaTime);
            speed = Random.Range(0.3f, 5f);
        }
        else if (Random.Range(0, 5) < 1)
        {
            GameObject[] goBoids;
            goBoids = OutsideGridFlock.boids;

            Vector3 vcenter = this.transform.position; //cohesion
            Vector3 vavoid = globalOrigin; //separation

            float gSpeed = 0.1f;
            Vector3 goalPos = OutsideGridFlock.goalPos;

            float dist;
            int groupSize = 0;

            foreach (GameObject go in goBoids)
            {
                if (go != this.gameObject)
                {
                    dist = Vector3.Distance(go.transform.position, this.transform.position);
                    if (dist <= neighbourDistance)
                    {
                        vcenter += go.transform.position;
                        groupSize++;
                        if (dist < 6f)
                        {
                            vavoid = vavoid + (this.transform.position - go.transform.position); //serparation calc
                        }

                        OutsideFlockEntity anotherFlockEntity = go.GetComponent<OutsideFlockEntity>();
                        gSpeed = gSpeed + anotherFlockEntity.speed;
                    }
                }
            }

            if (groupSize >= 0)
            {
                vcenter = vcenter / groupSize + (goalPos - this.transform.position); //cohesion calc
                speed = (gSpeed / groupSize) + Random.Range(-0.1f, 0.1f);
                if (speed > 8f)
                {
                    speed = Random.Range(0.5f, 3f);
                }

                Vector3 dir = (vcenter + vavoid) - this.transform.position; //alignment calc
                if (!float.IsInfinity(dir.x))
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), rotSpeed * Time.deltaTime);
                }
            }
        }

        transform.Translate(0, 0, Time.deltaTime * speed);
    }
}
