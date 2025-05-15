using UnityEngine;

public class HorizontalGhostBehavior : NewBehavior2
{
    private float time = 0f;
    public float moveSpeed = 1f;          // How fast the ghost cycles
    public float moveWidth = 2f;          // How far left/right from center
    public Vector2 centerPoint = Vector2.zero; // Center of motion

    protected override void Start()
    {
        base.Start();

        if (centerPoint == Vector2.zero)  // Only update if not set manually
            centerPoint = transform.position;

        isActive = true;
    }

    void FixedUpdate()
    {
        if (!isActive) return;

        time += Time.fixedDeltaTime * moveSpeed;

        // Oscillates between -1 and 1, then scaled by moveWidth
        float xOffset = Mathf.Sin(time) * moveWidth;

        // Calculates the ghost's new X position
        Vector2 targetPos = centerPoint + new Vector2(xOffset, 0);
        Vector2 direction = (targetPos - (Vector2)transform.position).normalized;

        HandleMove(direction, speed);
    }
}
