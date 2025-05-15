using UnityEngine;

public class VerticalGhostBehavior : NewBehavior2
{
    private float time = 0f;
    public float moveSpeed = 1f;
    public float moveHeight = 2f;
    public Vector2 centerPoint = Vector2.zero; // The center of the up-down motion

    protected override void Start()
    {
        base.Start();
        isActive = true;
        centerPoint = transform.position; // Save the starting position
    }

    void FixedUpdate()
    {
        if (!isActive) return;

        time += Time.fixedDeltaTime * moveSpeed;

        float yOffset = Mathf.Sin(time) * moveHeight;
        Vector2 targetPos = centerPoint + new Vector2(0, yOffset);
        Vector2 direction = (targetPos - (Vector2)transform.position).normalized;

        HandleMove(direction, speed);
    }
}
