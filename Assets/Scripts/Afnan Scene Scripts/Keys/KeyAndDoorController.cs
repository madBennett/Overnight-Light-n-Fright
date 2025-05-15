using UnityEngine;
using System.Collections;

public class KeyAndDoorController : MonoBehaviour
{
    private bool hasKey = false;

    public GameObject keyObject;
    public GameObject doorObject;
    public AudioClip keyPickupSound; // 🎧 The sound to play when key is collected

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collided with: " + other.name);

        if (other.CompareTag("Key"))
        {
            hasKey = true;
            keyObject.SetActive(false);
            Debug.Log("Key collected!");

            // 🔊 Play key pickup sound
            if (audioSource != null && keyPickupSound != null)
            {
                audioSource.PlayOneShot(keyPickupSound);
            }
        }

        if (other.CompareTag("Door"))
        {
            if (hasKey)
            {
                Debug.Log("Door unlocked!");
                doorObject.SetActive(false);
                StartCoroutine(ReappearDoorAfterDelay(2f)); // 🕑 reappear after 2 seconds
            }
            else
            {
                Debug.Log("Touched door, but no key yet.");
            }
        }
    }

    private IEnumerator ReappearDoorAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (doorObject != null)
        {
            doorObject.SetActive(true);
            Debug.Log("Door reappeared.");
        }
    }
}
