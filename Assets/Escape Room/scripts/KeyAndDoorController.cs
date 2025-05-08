using UnityEngine;

public class KeyAndDoorController : MonoBehaviour
{
    private bool hasKey = false;

    public GameObject keyObject;   // Drag the key GameObject here
    public GameObject doorObject;  // Drag the door GameObject here

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Key"))
        {
            hasKey = true;
            keyObject.SetActive(false); // Make the key disappear
        }

        if (other.CompareTag("Door"))
        {
            if (hasKey)
            {
                doorObject.SetActive(false); // Open the door
            }
        }
    }
}
