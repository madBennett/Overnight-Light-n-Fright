using UnityEngine;

public class GhostStateMachine : MonoBehaviour
{
    public enum GhostState { Idle, Chase }

    public float moveSpeed = 2f;
    private GhostState currentState = GhostState.Idle;

    private Transform player;
    private Rigidbody2D rb;
    private Animator animator;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); //  reference ghost animator
    }

    public void StartChasing()
    {
        currentState = GhostState.Chase;

        if (animator != null)
        {
            animator.SetTrigger("Chase"); //  trigger your chase animation
        }
    }

    void FixedUpdate()
    {
        if (currentState == GhostState.Chase && player != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
        }
    }
}
