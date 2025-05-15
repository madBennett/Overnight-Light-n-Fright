using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovingPlatform : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float moveSpeed = 4f;
    public float pauseDuration = 1f;

    private Vector3 nextPosition;
    private bool isWaiting = false;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.simulated = true;

        nextPosition = pointB.position;
    }

    void FixedUpdate()
    {
        if (isWaiting) return;

        Vector3 newPos = Vector3.MoveTowards(transform.position, nextPosition, moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);

        if (newPos == nextPosition)
        {
            StartCoroutine(WaitBeforeMoving());
        }
    }

    private IEnumerator WaitBeforeMoving()
    {
        isWaiting = true;
        yield return new WaitForSeconds(pauseDuration);
        nextPosition = (nextPosition == pointA.position) ? pointB.position : pointA.position;
        isWaiting = false;
    }
}
