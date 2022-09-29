using UnityEngine;

public class Move : MonoBehaviour {

    Vector3 goal = new Vector3(5, 0, 4);

    void Start() {

        this.transform.Translate(goal);
    }

    private void Update() {

    }
}
