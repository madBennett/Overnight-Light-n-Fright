using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private Vector3 spawnPoint;
    private Rigidbody2D rb;

    void Start()
    {
        spawnPoint = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    public void Respawn()
    {
        transform.position = spawnPoint;
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
        }
    }
}
