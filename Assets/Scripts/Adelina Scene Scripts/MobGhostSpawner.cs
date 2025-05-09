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

    [Header("Wave Settings")]
    public int maxWaves = 5;                       // total number of waves
    public string taskIDToComplete = "ShootTask";  // task ID to mark complete
    public TeleportDoor teleportDoor;              // the door to lock/unlock
    public ReturnToLobby returnToLobby;            // return to lobby

    // track current wave
    private List<GameObject> activeGhosts = new List<GameObject>();
    private int currentWave = 0;
    private bool allWavesComplete = false;

    void Start()
    {
        LockDoors();
        SpawnGhostsInCircle();
    }

    void SpawnGhostsInCircle()
    {
        if (allWavesComplete) return;

        currentWave++;
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
    }

    private void OnGhostDespawned(MobGhostBehavior ghost)
    {
        activeGhosts.Remove(ghost.gameObject);

        if (activeGhosts.Count == 0)
        {
            if (currentWave >= maxWaves)
            {
                CompleteShootTask();
            }
            else
            {
                // increase difficulty
                numberOfGhosts += 2;
                speedMultiplier += 0.25f;

                // spawn next wave
                SpawnGhostsInCircle();
            }
        }
    }


    private void CompleteShootTask()
    {
        allWavesComplete = true;
        Debug.Log("[MobGhostSpawner] All waves complete. Completing task and unlocking doors.");

        if (PlayerProgress.Instance != null)
        {
            PlayerProgress.Instance.CompleteTask(taskIDToComplete);
        }

        UnlockDoors();
    }

    private void LockDoors()
    {
        if (teleportDoor != null)
            teleportDoor.GetComponent<Collider2D>().enabled = false;

        if (returnToLobby != null)
            returnToLobby.GetComponent<Collider2D>().enabled = false;
    }

        private void UnlockDoors()
    {
        if (teleportDoor != null)
            teleportDoor.GetComponent<Collider2D>().enabled = true;

        if (returnToLobby != null)
            returnToLobby.GetComponent<Collider2D>().enabled = true;
    }
}
