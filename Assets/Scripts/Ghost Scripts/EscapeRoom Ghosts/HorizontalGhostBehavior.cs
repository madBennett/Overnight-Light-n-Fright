using UnityEngine;

public class HorizontalGhostBehavior : NewBehavior2
{
    public float patrolDistance = 0.5f;     // How far left/right to go from the starting point
    private Vector2 startPoint;
    private int direction = 1;            // 1 = right, -1 = left

    protected override void Start()
    {
        base.Start();
        isActive = true;

        // Remember where the ghost starts
        startPoint = transform.position;
    }

    void FixedUpdate()
    {
        if (!isActive) return;

        float targetX = startPoint.x + patrolDistance * direction;
        float currentX = transform.position.x;

        // If reached or passed the target, flip direction
        if ((direction == 1 && currentX >= targetX) || (direction == -1 && currentX <= targetX))
        {
            direction *= -1;
        }

        // Move horizontally based on direction
        Vector2 moveDir = new Vector2(direction, 0).normalized;
        HandleMove(moveDir, speed);
    }
}
