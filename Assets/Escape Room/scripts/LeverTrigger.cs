using UnityEngine;

public class LeverTrigger : MonoBehaviour
{
    public GameObject gateObject;  // The door that should disappear

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Lever"))
        {
            gateObject.SetActive(false);  // Make the gate disappear
        }
    }
}
