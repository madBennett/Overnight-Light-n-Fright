using UnityEngine;

public class GhostCollisionHandler : MonoBehaviour
{
    private GhostResetAfterDelay resetScript;
    private SimpleGhostChase chaseScript;

    private void Start()
    {
        resetScript = GetComponent<GhostResetAfterDelay>();
        chaseScript = GetComponent<SimpleGhostChase>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            resetScript?.ResetToSpawn();
            chaseScript?.StopChase(); // Optional: pause chasing during reset delay
        }
    }
}
