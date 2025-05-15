using UnityEngine;
using System.Collections;

public class GhostResetAfterDelay : MonoBehaviour
{
    public float resetDelay = 5f;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    public void ResetToSpawn()
    {
        StartCoroutine(ResetAfterDelay());
    }

    private IEnumerator ResetAfterDelay()
    {
        yield return new WaitForSeconds(resetDelay);

        transform.position = originalPosition;
        transform.rotation = originalRotation;

        Debug.Log("[GhostResetAfterDelay] Ghost reset to original position.");
    }
}
