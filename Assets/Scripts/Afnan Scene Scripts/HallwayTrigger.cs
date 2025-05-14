using UnityEngine;

public class HallwayTrigger : MonoBehaviour
{
    public ChasingGhostBehavior ghostToActivate;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ghostToActivate.SetPlayerInHallway(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ghostToActivate.SetPlayerInHallway(false);
        }
    }
}
