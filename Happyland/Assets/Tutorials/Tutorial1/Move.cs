using UnityEngine;

public class Move : MonoBehaviour{

    public float speed = 2.0f;

    public float accuracy = 0.1f;

    public Transform goal;

    void Start()
    {
    }

    private void Update()
    {
        this.transform.LookAt(goal.position);
        Vector3 direction = goal.position - this.transform.position;
        Debug.DrawRay(this.transform.position, direction, Color.red);
        if (direction.magnitude > 1.0f)
        {
            this.transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
        }
    }
}
