using UnityEngine;

public class ChasingGhostBehavior : AbstractGhostBehavior
{
    public float chaseSpeed = 3f;
    private bool playerInHallway = false;
    private Vector2 lastDirection = Vector2.zero;

    protected override void Start()
    {
        base.Start();
        isActive = true;
    }

    void FixedUpdate()
    {
        if (!isActive || !playerInHallway || Player == null) return;

        // Only chase downward
        Vector2 direction = ((Vector2)Player.transform.position - (Vector2)transform.position);

        if (direction.y < 0)
        {
            lastDirection = new Vector2(0, -1);
        }

        HandleMove(lastDirection, chaseSpeed);
    }

    public void SetPlayerInHallway(bool inHallway)
    {
        playerInHallway = inHallway;

        if (!inHallway)
        {
            // Stop when player leaves hallway
            HandleMove(Vector2.zero, 0f);
        }
    }
}
