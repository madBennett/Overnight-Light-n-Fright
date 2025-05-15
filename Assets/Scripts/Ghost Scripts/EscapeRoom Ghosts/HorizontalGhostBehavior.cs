using UnityEngine;

public class HorizontalGhostBehavior : NewBehavior2
{
    private float time = 0f;
    public float moveSpeed = 1f;      // Speed of looping motion
    public float moveWidth = 2f;      // Horizontal distance to move
    public Vector2 centerPoint = Vector2.zero; // Starting center point

    protected override void Start()
    {
        base.Start();
        isActive = true;
        centerPoint = transform.position; // Start at placed position
    }

    void FixedUpdate()
    {
        if (!isActive) return;

        time += Time.fixedDeltaTime * moveSpeed;

        float xOffset = Mathf.Sin(time) * moveWidth;
        Vector2 targetPos = centerPoint + new Vector2(xOffset, 0);
        Vector2 direction = (targetPos - (Vector2)transform.position).normalized;

        HandleMove(direction, speed);
    }
}
