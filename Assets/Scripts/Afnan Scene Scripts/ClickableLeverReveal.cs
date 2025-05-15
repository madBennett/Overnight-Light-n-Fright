using UnityEngine;

public class ClickableLeverReveal : MonoBehaviour
{
    public GameObject objectToReveal;

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            if (objectToReveal != null)
            {
                objectToReveal.SetActive(true);
                triggered = true;
                Debug.Log("Auto Lever triggered — object revealed!");
            }
        }
    }
}
