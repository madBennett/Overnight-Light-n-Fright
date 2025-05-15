using UnityEngine;

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
                doorObject.SetActive(false);
                Debug.Log("Door unlocked!");
            }
            else
            {
                Debug.Log("Touched door, but no key yet.");
            }
        }
    }
}
