using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobGhostSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject mobGhostPrefab;  // ghost prefabs to spawn
    public int numberOfGhosts = 8;     // total ghosts to spawn
    public float spawnRadius = 5f;     // radius of the circle

    void Start()
    {
        SpawnGhostsInCircle();
    }

    void SpawnGhostsInCircle()
    {
        Vector3 center = Vector3.zero; // center of room

        for (int i = 0; i < numberOfGhosts; i++)
        {
            float angle = i * Mathf.PI * 2f / numberOfGhosts;
            Vector3 spawnPos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * spawnRadius + center;
            Instantiate(mobGhostPrefab, spawnPos, Quaternion.identity);
        }
    }
}
