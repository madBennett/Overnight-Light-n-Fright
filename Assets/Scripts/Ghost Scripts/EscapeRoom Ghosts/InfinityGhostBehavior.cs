using UnityEngine;

public class InfinityGhostBehavior : AbstractGhostBehavior
{
    private float time = 0f;
    public float loopSpeed = 1f;
    public float loopSize = 2f;
    public Vector2 centerPoint = Vector2.zero; // Center of the figure-8 motion

    protected override void Start()
    {
        base.Start();
        isActive = true;
        centerPoint = transform.position; // Start where the ghost is placed
    }

    void FixedUpdate()
    {
        if (!isActive) return;

        time += Time.fixedDeltaTime * loopSpeed;

        float x = Mathf.Sin(time) * loopSize;
        float y = (Mathf.Sin(2 * time) / 2f) * loopSize;

        Vector2 targetPos = centerPoint + new Vector2(x, y);
        Vector2 direction = (targetPos - (Vector2)transform.position).normalized;

        HandleMove(direction, speed);
    }
}
