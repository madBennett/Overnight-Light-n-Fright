using UnityEngine;

public class ChasingGhostBehavior : AbstractGhostBehavior
{
    private bool playerInHallway = false;

    protected override void Start()
    {
        base.Start();
        isActive = true;
    }

    void FixedUpdate()
    {
        if (!isActive || !playerInHallway || Player == null) return;

        // Only chase vertically
        Vector2 direction = ((Vector2)Player.transform.position - (Vector2)transform.position);
        direction.x = 0;
        direction = direction.normalized;

        HandleMove(direction, speed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInHallway = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInHallway = false;
            HandleMove(Vector2.zero, 0f); // Stop moving when player leaves
        }
    }
}
