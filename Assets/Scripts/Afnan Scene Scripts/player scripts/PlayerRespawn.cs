using UnityEngine;
using System.Collections;

public class PlayerRespawn : MonoBehaviour
{
    private Vector3 spawnPoint;
    private Rigidbody2D rb;
    private JitterShaderController jitterController;

    void Start()
    {
        spawnPoint = transform.position;
        rb = GetComponent<Rigidbody2D>();
        jitterController = FindObjectOfType<JitterShaderController>();
    }

    public void Respawn()
    {
        transform.position = spawnPoint;
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
        }

        if (jitterController != null)
        {
            jitterController.TriggerJitter(); // ✅ trigger the effect
        }
    }
}
