using UnityEngine;

public class GhostActivationZone : MonoBehaviour
{
    public SimpleGhostChase[] ghostsToActivate;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (SimpleGhostChase ghost in ghostsToActivate)
            {
                if (ghost != null)
                    ghost.ActivateChase();
            }

            Destroy(gameObject); // Optional: use once only
        }
    }
}
