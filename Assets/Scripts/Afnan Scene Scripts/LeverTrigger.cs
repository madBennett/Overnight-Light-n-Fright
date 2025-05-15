using UnityEngine;
using System.Collections;

public class LeverTrigger : MonoBehaviour
{
    public GameObject gateObject;            // The door to open/close
    public float gateOpenDuration = 1.5f;    // How long the door stays open
    public Animator leverAnimator;           // Lever animation controller
    private AudioSource audioSource;

    private bool hasBeenPulled = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Ensure door starts closed
        if (gateObject != null)
            gateObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasBeenPulled && other.CompareTag("Player"))
        {
            hasBeenPulled = true;

            // Animate lever
            if (leverAnimator != null)
                leverAnimator.SetTrigger("Flip");

            // Play sound
            if (audioSource != null && audioSource.clip != null)
                audioSource.Play();

            // Open door and start reset
            if (gateObject != null)
                gateObject.SetActive(false); // "Open" the door by hiding it

            StartCoroutine(ResetLeverAndCloseDoor());
        }
    }

    private IEnumerator ResetLeverAndCloseDoor()
    {
        yield return new WaitForSeconds(gateOpenDuration);

        // Close the door again
        if (gateObject != null)
            gateObject.SetActive(true); // "Close" the door

        hasBeenPulled = false; // Lever becomes reusable again
    }
}
