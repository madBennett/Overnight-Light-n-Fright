using UnityEngine;
using System.Collections;

public class LeverTrigger : MonoBehaviour
{
    public GameObject gateObject;           // The gate to open/close
    public float gateOpenDuration = 0.25f;     // How long the gate stays open
    public Animator leverAnimator;          // Lever animation controller
    private AudioSource audioSource;

    private bool hasBeenPulled = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        //  Ensure the gate is active at start
        if (gateObject != null && !gateObject.activeSelf)
        {
            gateObject.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasBeenPulled && other.CompareTag("Player"))
        {
            hasBeenPulled = true;

            // Play animation
            if (leverAnimator != null)
                leverAnimator.SetTrigger("Flip");

            // Play sound
            if (audioSource != null && audioSource.clip != null)
                audioSource.Play();

            // Open gate for a duration
            StartCoroutine(OpenThenCloseGate());
        }
    }

    private IEnumerator OpenThenCloseGate()
    {
        gateObject.SetActive(false); // Open
        yield return new WaitForSeconds(gateOpenDuration);
        gateObject.SetActive(true);  // Close
    }
}
