using UnityEngine;
using System.Collections;

public class LeverTrigger : MonoBehaviour
{
    public Animator leverAnimator;
    public AudioSource audioSource;
    public DoorController targetDoor;   // ✅ Specific door to open
    public float resetDelay = 1f;

    private bool hasBeenPulled = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasBeenPulled && other.CompareTag("Player"))
        {
            hasBeenPulled = true;

            leverAnimator?.SetTrigger("Flip");
            if (audioSource != null && audioSource.clip != null)
                audioSource.Play();

            // ✅ Open specific door
            targetDoor?.Open();

            StartCoroutine(ResetLeverAfterDelay());
        }
    }

    private IEnumerator ResetLeverAfterDelay()
    {
        yield return new WaitForSeconds(resetDelay);
        hasBeenPulled = false;
    }
}
