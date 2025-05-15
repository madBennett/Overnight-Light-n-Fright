using UnityEngine;

public class GhostKillsPlayer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerRespawn respawn = other.GetComponent<PlayerRespawn>();
            if (respawn != null)
            {
                respawn.Respawn();
                Debug.Log("[GhostKillsPlayer] Player touched ghost and respawned.");
            }
        }
    }
}
