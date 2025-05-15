using UnityEngine;

public class ClickableLeverReveal : MonoBehaviour
{
    public GameObject objectToReveal;       // The hidden sprite or object
    public Animator leverAnimator;          // Lever Animator
    private AudioSource audioSource;

    private bool hasBeenPulled = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Ensure the reveal object starts hidden
        if (objectToReveal != null)
            objectToReveal.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasBeenPulled && other.CompareTag("Player"))
        {
            hasBeenPulled = true;

            // Trigger the lever flip animation
            if (leverAnimator != null)
                leverAnimator.SetTrigger("Flip");

            // Play audio if available
            if (audioSource != null && audioSource.clip != null)
                audioSource.Play();

            // Reveal the object
            if (objectToReveal != null)
                objectToReveal.SetActive(true);
        }
    }
}
