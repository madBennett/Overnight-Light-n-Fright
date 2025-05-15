using System.Collections;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float moveSpeed = 4f;
    public float pauseDuration = 1f; // How long to wait at each end

    private Vector3 nextPosition;
    private bool isWaiting = false;

    void Start()
    {
        nextPosition = pointB.position;
    }

    void Update()
    {
        if (isWaiting) return;

        transform.position = Vector3.MoveTowards(transform.position, nextPosition, moveSpeed * Time.deltaTime);

        if (transform.position == nextPosition)
        {
            StartCoroutine(WaitBeforeMoving());
        }
    }

    private IEnumerator WaitBeforeMoving()
    {
        isWaiting = true;

        // Wait at current point
        yield return new WaitForSeconds(pauseDuration);

        // Switch to the other point
        nextPosition = (nextPosition == pointA.position) ? pointB.position : pointA.position;

        isWaiting = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.parent = transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.parent = null;
        }
    }
}
