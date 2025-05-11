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

    [SerializeField] private AudioManager AM;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        AM = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();

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

            AM.PlayAudio(AudioClipTypes.ENTER_GATE);

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
