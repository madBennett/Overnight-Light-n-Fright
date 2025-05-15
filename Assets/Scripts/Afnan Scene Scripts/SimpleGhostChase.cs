using UnityEngine;

public class SimpleGhostChase : NewBehavior2
{
    public float moveSpeed = 2f;

    protected override void Start()
    {
        base.Start();
        isActive = false; // ghost waits until activated
    }

    void FixedUpdate()
    {
        if (!isActive || Player == null) return;

        Vector2 direction = ((Vector2)Player.transform.position - (Vector2)transform.position).normalized;
        HandleMove(direction, moveSpeed);
    }

    public void ActivateChase()
    {
        isActive = true;
    }

    public void StopChase()
    {
        isActive = false;
        HandleMove(Vector2.zero, 0f); // Stop ghost and animation
    }
}
