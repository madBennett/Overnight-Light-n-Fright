using UnityEngine;

public class KeyAndDoorController : MonoBehaviour
{
    private bool hasKey = false;

    public GameObject keyObject;
    public GameObject doorObject;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collided with: " + other.name); // NEW

        if (other.CompareTag("Key"))
        {
            hasKey = true;
            keyObject.SetActive(false);
            Debug.Log("Key collected!"); // NEW
        }

        if (other.CompareTag("Door"))
        {
            if (hasKey)
            {
                doorObject.SetActive(false);
                Debug.Log("Door unlocked!"); // NEW
            }
            else
            {
                Debug.Log("Touched door, but no key yet."); // NEW
            }
        }
    }
}
