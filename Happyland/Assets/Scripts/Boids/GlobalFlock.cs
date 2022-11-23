using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalFlock : MonoBehaviour
{
    public GameObject boidPrefab;
    public int waitTimeBetweenGoalChange = 10;
    public static float minFlockHeight = 0.4f;
    public static int airSizeX = 58;
    public static int airSizeY = 38;
    public static int numBirds = 8;
    public static GameObject[] boids = new GameObject[numBirds];
    public static Vector3 goalPos = new Vector3(0, 0, 0);

    GameObject player;
    Vector3 playerPos;

    private IEnumerator setGoalPos()
    {
        yield return new WaitForSeconds(waitTimeBetweenGoalChange);

        goalPos = new Vector3(Random.Range(transform.position.x - (airSizeX / 2), transform.position.x + (airSizeX / 2)), Random.Range(transform.position.y + minFlockHeight, transform.position.y + minFlockHeight + 0.5f), Random.Range(transform.position.z - (airSizeY / 2), transform.position.z + (airSizeY / 2)));
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerPos = player.transform.position;
        goalPos = new Vector3(Random.Range(transform.position.x - (airSizeX / 2), transform.position.x + (airSizeX / 2)), Random.Range(transform.position.y + minFlockHeight, transform.position.y + minFlockHeight + 0.5f), Random.Range(transform.position.z - (airSizeY / 2), transform.position.z + (airSizeY / 2)));

        for (int i = 0; i < numBirds; i++)
        {
            Vector3 pos = new Vector3(Random.Range(transform.position.x - (airSizeX / 2), transform.position.x + (airSizeX / 2)), Random.Range(transform.position.y + minFlockHeight, transform.position.y + minFlockHeight  + 0.5f), Random.Range(transform.position.z - (airSizeY / 2), transform.position.z + (airSizeY / 2)));

            boids[i] = (GameObject)Instantiate(boidPrefab, pos, Quaternion.LookRotation(new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1))));
        }

        StartCoroutine(setGoalPos());
    }
}
