using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportDoor : MonoBehaviour
{
    public string sceneToLoad;
    public string doorID;
    public string requiredTaskID; // NEW: set in Inspector if needed
    public DoorData doorData;

    private SpriteRenderer spriteRenderer;
    public Color lockedColor = Color.white; // Color when door locks

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (PlayerProgress.Instance != null && PlayerProgress.Instance.HasCompletedTask(requiredTaskID))
        {
            LockDoor();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (PlayerProgress.Instance != null && PlayerProgress.Instance.HasCompletedTask(requiredTaskID))
            {
                // Door is locked, cannot use
                return;
            }

            doorData.lastDoorUsed = doorID;
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    private void LockDoor()
    {
        // Disable Collider
        GetComponent<Collider2D>().enabled = false;

        // Change color
        if (spriteRenderer != null)
        {
            spriteRenderer.color = lockedColor;
        }
    }
}
