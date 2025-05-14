using UnityEngine;
using System.Collections;

public class LeverTrigger : MonoBehaviour
{
    public GameObject gateObject;  // The gate to disappear
    public float hideDuration = 0.25f; // Duration to hide the gate

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Lever"))
        {
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
