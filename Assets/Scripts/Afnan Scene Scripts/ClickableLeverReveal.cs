using UnityEngine;

public class ClickableLeverReveal : MonoBehaviour
{
    public GameObject objectToReveal;  // Assign the hidden sprite here
    public KeyCode interactKey = KeyCode.E;

    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(interactKey))
        {
            if (objectToReveal != null)
            {
                objectToReveal.SetActive(true);
                Debug.Log("Lever clicked! Revealed object.");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
