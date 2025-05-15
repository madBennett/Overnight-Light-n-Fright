using UnityEngine;

public class TriggerZone : MonoBehaviour
{
    public GhostStateMachine ghostToActivate;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && ghostToActivate != null)
        {
            ghostToActivate.StartChasing();
            Destroy(gameObject); // Optional: remove trigger after use
        }
    }
}
