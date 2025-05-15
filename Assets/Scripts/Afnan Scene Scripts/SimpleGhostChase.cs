using UnityEngine;

public class SimpleGhostChase : MonoBehaviour
{
    public float moveSpeed = 2f;
    private bool isChasing = false;

    private Transform player;
    private Rigidbody2D rb;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (!isChasing || player == null) return;

        Vector2 direction = ((Vector2)player.position - rb.position).normalized;
        rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
    }

    public void ActivateChase()
    {
        isChasing = true;
    }

    public void StopChase()
    {
        isChasing = false;
    }
}
