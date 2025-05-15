using UnityEngine;
using System.Collections;

public class LeverTrigger : MonoBehaviour
{
    public GameObject gateObject;           // The gate to disappear
    public float hideDuration = 0.25f;      // Duration to hide the gate
    public Animator leverAnimator;          // The Animator component on the lever

    private bool hasBeenPulled = false;     // To prevent repeated triggers

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasBeenPulled && other.CompareTag("Player"))
        {
            hasBeenPulled = true;

            // Trigger the flip animation
            if (leverAnimator != null)
            {
                leverAnimator.SetTrigger("Flip");
            }

            // Start the gate hide routine
            StartCoroutine(HideGateTemporarily());
        }
    }

    private IEnumerator HideGateTemporarily()
    {
        gateObject.SetActive(false);
        yield return new WaitForSeconds(hideDuration);
        gateObject.SetActive(true);
    }
}
