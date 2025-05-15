using UnityEngine;

public class ChasingGhostBehavior : AbstractGhostBehavior
{
    public float chaseSpeed = 4f;
    private bool playerInHallway = false;
    private Vector2 lastDirection = Vector2.zero;
    private AudioSource audioSource;
    private bool audioPlayed = false;

    protected override void Start()
    {
        base.Start();
        isActive = true;
        audioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        if (!isActive || !playerInHallway || Player == null) return;

        Vector2 direction = ((Vector2)Player.transform.position - (Vector2)transform.position);

        if (direction.y < 0)
        {
            lastDirection = new Vector2(0, -1);

            // Play audio only once when starting to chase
            if (!audioPlayed && audioSource != null && audioSource.clip != null)
            {
                audioSource.Play();
                audioPlayed = true;
            }
        }

        HandleMove(lastDirection, chaseSpeed);
    }

    public void SetPlayerInHallway(bool inHallway)
    {
        playerInHallway = inHallway;

        if (!inHallway)
        {
            // Stop movement and reset audio flag
            HandleMove(Vector2.zero, 0f);
            audioPlayed = false;
        }
    }
}
