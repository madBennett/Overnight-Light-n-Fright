using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public GameObject keyObject; // Assign in Inspector
    public GameObject doorObject; // Assign in Inspector

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == keyObject)
        {
            // Disable the key and the door
            keyObject.SetActive(false);
            doorObject.SetActive(false);
        }
    }
}
