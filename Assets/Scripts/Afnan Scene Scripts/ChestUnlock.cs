using UnityEngine;

public class ChestUnlock : MonoBehaviour
{
    public Animator chestAnimator;
    public AudioSource audioSource;
    public AudioClip openSound;

    public GameObject doorToOpen;  
    private bool isOpened = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isOpened && other.CompareTag("Player") && WavyKey.keyCollected)
        {
            isOpened = true;

            if (chestAnimator != null)
                chestAnimator.SetTrigger("Treasure");

            if (audioSource != null && openSound != null)
                audioSource.PlayOneShot(openSound);

            if (doorToOpen != null)
                doorToOpen.SetActive(false); 

            Debug.Log("Chest opened! Door triggered.");
        }
    }
}
