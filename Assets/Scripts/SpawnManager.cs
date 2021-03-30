using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager insanse;

    Spawner[] spawnpoints;
    
    void Awake()
    {
        insanse = this;
        spawnpoints = GetComponentsInChildren<Spawner>();
    }

    public Transform GetSpawnPoint()
    {
        return spawnpoints[Random.Range(0, spawnpoints.Length)].transform;
    }
}
