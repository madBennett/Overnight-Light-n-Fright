using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbySpawnManager : MonoBehaviour
{
    public DoorData doorData;
    public Transform defaultSpawnPoint;
    public Transform[] doorSpawnPoints;

    // Match this with the doorID from TeleportDoor
    public string[] doorIDs;

    void Start()
    {
        Transform spawnPoint = defaultSpawnPoint;

        for (int i = 0; i < doorIDs.Length; i++)
        {
            if (doorData.lastDoorUsed == doorIDs[i])
            {
                spawnPoint = doorSpawnPoints[i];
                break;
            }
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = spawnPoint.position;
        }

        doorData.lastDoorUsed = ""; // Reset
    }
}
