using UnityEngine;

[RequireComponent(typeof(Animator))]
public class GhostWavyMotion : MonoBehaviour
{
    public float waveSpeed = 2f;       // Speed of vertical wave motion
    public float waveHeight = 0.5f;    // Height of the vertical wave
    public float moveSpeed = 1f;       // Horizontal movement speed
    public float moveRange = 2f;       // How far to move left/right from start

    private Vector3 startPos;
    private bool movingRight = true;
    private Animator animator;

    void Start()
    {
        startPos = transform.position;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Vertical sine wave motion
        float waveY = Mathf.Sin(Time.time * waveSpeed) * waveHeight;

        // Horizontal float left/right
        float direction = movingRight ? 1 : -1;
        transform.position += new Vector3(direction * moveSpeed * Time.deltaTime, waveY * Time.deltaTime, 0);

        // Flip direction if ghost reaches horizontal bounds
        if (transform.position.x > startPos.x + moveRange)
        {
            movingRight = false;
            FlipSprite(false);
        }
        else if (transform.position.x < startPos.x - moveRange)
        {
            movingRight = true;
            FlipSprite(true);
        }
    }

    void FlipSprite(bool faceRight)
    {
        if (animator != null)
        {
            animator.SetBool("FacingRight", faceRight); // ✅ Optional for anim blend
        }

        // Optional: flip visual only
        Vector3 scale = transform.localScale;
        scale.x = faceRight ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        transform.localScale = scale;
    }
}
