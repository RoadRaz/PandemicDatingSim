using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdSpawner : MonoBehaviour
{
    // prefabs
    public GameObject crowdMember;
    public GameObject infectedCrowdMember;

    // spawning bounds
    float xMin;
    float xMax;
    float yMin;
    float yMax;

    // constant number for amount of crowd to spawn
    const int CROWD_NUM = 40;

    // Start is called before the first frame update
    void Start()
    {
        xMin = -2;
        xMax = 2;
        yMin = -3;
        yMax = 3;

        // Spawn not infected crowd members
        for(int i = 0; i < CROWD_NUM; i++)
        {
            Vector3 randomPos = new Vector3(Random.Range(xMin, xMax), Random.Range(yMin, yMax), 0.0f);

            SpawnCrowdMember(randomPos);
        }

        // Spawn 1 infected crowd member 
        Vector3 randomInfectedPos = new Vector3(Random.Range(xMin, xMax), Random.Range(yMin, yMax), 0.0f);

        SpawnInfectedCrowdMember(randomInfectedPos);
    }

    void SpawnCrowdMember(Vector3 pos)
    {
        Instantiate(crowdMember, pos, Quaternion.identity);
    }

    void SpawnInfectedCrowdMember(Vector3 pos)
    {
        Instantiate(infectedCrowdMember, pos, Quaternion.identity);
    }
}
