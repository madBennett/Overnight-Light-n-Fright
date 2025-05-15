using UnityEngine;

public class NewSimpleGhostChase : NewBehavior2
{
    public float moveSpeed = 2f;
    private bool isChasing = false;
    private Vector3 originalPosition;

    protected override void Start()
    {
        base.Start(); // Call base class to set up rigidBody, animator, Player, etc.
        originalPosition = transform.position;
    }

    void FixedUpdate()
    {
        if (isChasing && Player != null)
        {
            Vector2 direction = ((Vector2)Player.transform.position - (Vector2)transform.position).normalized;
            HandleMove(direction, moveSpeed);
        }
    }

    public void ActivateChase()
    {
        isChasing = true;
    }

    public void ResetGhost()
    {
        isChasing = false;
        HandleMove(Vector2.zero, 0f); // Stop movement and animation
        transform.position = originalPosition;
    }
}
