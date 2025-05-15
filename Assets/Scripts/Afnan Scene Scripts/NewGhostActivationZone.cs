using System.Collections;
using UnityEngine;

public class NewGhostActivationZone : MonoBehaviour
{
    public NewSimpleGhostChase[] ghostsToActivate;
    private bool canTrigger = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!canTrigger || !other.CompareTag("Player"))
            return;

        StartCoroutine(ResetAndReactivateGhosts());
    }

    private IEnumerator ResetAndReactivateGhosts()
    {
        canTrigger = false;

        foreach (NewSimpleGhostChase ghost in ghostsToActivate)
        {
            if (ghost != null)
                ghost.ResetGhost(); // move back & stop chase
        }

        yield return new WaitForSeconds(5f);

        foreach (NewSimpleGhostChase ghost in ghostsToActivate)
        {
            if (ghost != null)
                ghost.ActivateChase();
        }

        canTrigger = true;
    }
}
