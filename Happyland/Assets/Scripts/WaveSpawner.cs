using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public GameObject[] enemiesToSpawn;
}

public class WaveSpawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public Wave[] waves;

    private int currentWave;
    public int enemiesLeft;

    // Start is called before the first frame update
    void Start()
    {
        currentWave = 0;
        foreach (GameObject obj in waves[0].enemiesToSpawn)
        {
            GameObject spawned = Instantiate(obj, spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position, spawnPoints[Random.Range(0, spawnPoints.Length)].transform.rotation, null);
            enemiesLeft++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(enemiesLeft <= 0 && currentWave <= (waves.Length-1))
        {
            currentWave++;
            foreach (GameObject obj in waves[currentWave].enemiesToSpawn)
            {
                GameObject spawned = Instantiate(obj, spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position, spawnPoints[Random.Range(0, spawnPoints.Length)].transform.rotation, null);
                enemiesLeft++;
            }
        }

        //Trigger door anim here.
    }
}
