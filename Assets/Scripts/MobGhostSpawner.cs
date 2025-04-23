using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobGhostSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject mobGhostPrefab;   // ghost prefab to spawn
    public int numberOfGhosts = 8;      // total ghosts to spawn
    public float spawnRadius = 5f;      // radius of the circle
    public float speedMultiplier = 1f;  // increase speed on wave

    // track current wave
    private List<GameObject> activeGhosts = new List<GameObject>();

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

            GameObject ghost = Instantiate(mobGhostPrefab, spawnPos, Quaternion.identity);
            MobGhostBehavior behavior = ghost.GetComponent<MobGhostBehavior>();
            if (behavior != null)
            {
                behavior.speed *= speedMultiplier;
                behavior.chaseSpeed *= speedMultiplier;
                behavior.onGhostDespawned += OnGhostDespawned;
            }

            activeGhosts.Add(ghost);
        }


        // for (int i = 0; i < numberOfGhosts; i++)
        // {
        //     float angle = i * Mathf.PI * 2f / numberOfGhosts;
        //     Vector3 spawnPos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * spawnRadius + center;
        //     Instantiate(mobGhostPrefab, spawnPos, Quaternion.identity);
        // }
    }

    private void OnGhostDespawned(MobGhostBehavior ghost)
    {
        activeGhosts.Remove(ghost.gameObject);

        if (activeGhosts.Count == 0)
        {
            // Increase difficulty
            numberOfGhosts += 2;
            speedMultiplier += 0.25f;

            // Spawn next wave
            SpawnGhostsInCircle();
        }
    }
}
