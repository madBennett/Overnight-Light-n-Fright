using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportDoor : MonoBehaviour
{
    public string sceneToLoad;
    public string doorID;
    public string requiredTaskID;
    public DoorData doorData;

    private SpriteRenderer spriteRenderer;
    public Color unlockedColor; // assign this in Inspector
    public Color lockedColor = Color.white; // Color when door locks

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (PlayerProgress.Instance != null && PlayerProgress.Instance.HasCompletedTask(requiredTaskID))
        {
            LockDoor();
        }
    }

    private void Update()
    {
        Debug.Log(PlayerProgress.Instance.HasCompletedTask(requiredTaskID));
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

            if (LevelLoader.Instance != null)
            {
                LevelLoader.Instance.LoadScene(sceneToLoad);
            }
            else
            {
                SceneManager.LoadScene(sceneToLoad);
            }

        }
    }

    public void LockDoor()
    {
        // Disable Collider
        GetComponent<Collider2D>().enabled = false;

        // Change color
        if (spriteRenderer != null)
        {
            spriteRenderer.color = lockedColor;
        }
    }

    public void UnlockDoor()
    {
        // Enable Collider
        GetComponent<Collider2D>().enabled = true;

        // Change color
        if (spriteRenderer != null)
        {
            spriteRenderer.color = unlockedColor;
        }
    }
}
