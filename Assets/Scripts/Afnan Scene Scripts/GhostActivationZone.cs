using UnityEngine;

public class GhostActivationZone : MonoBehaviour
{
    public SimpleGhostChase ghostToActivate;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && ghostToActivate != null)
        {
            ghostToActivate.ActivateChase();
            Destroy(gameObject); // Optional: remove the trigger after activation
        }
    }
}
