using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportDoor : MonoBehaviour
{
    public string sceneToLoad;
    public string doorID;
    public DoorData doorData;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            doorData.lastDoorUsed = doorID;
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
