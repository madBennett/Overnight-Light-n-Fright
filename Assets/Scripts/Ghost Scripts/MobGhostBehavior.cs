using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobGhostBehavior : AbstractGhostBehavior
{
    public float moveDistance = 1f;
    public float detectionRange = 5f;
    private Vector3 moveDestination;
    public ParticleSystem despawnParticles;
    private SnowEffectController snowEffect;

    public float chaseSpeed = 3f;
    public float acceleration = 5f;
    private float currentSpeed;

    private SpriteRenderer spriteRenderer;
    private Color defaultColor = Color.white; // or whatever its default color is
    private Color chaseColor = Color.red;

    protected override void Start()
    {
        base.Start();
        isActive = true;
        Player = GameObject.FindGameObjectWithTag("Player");
        snowEffect = Camera.main.GetComponent<SnowEffectController>();
        currentSpeed = speed; // start at normal speed

        // Get SpriteRenderer
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            defaultColor = spriteRenderer.color;
        }
    }

    private void Update()
    {
        switch (currState)
        {
            case GhostStates.IDLE:
                Idle();
                break;
            case GhostStates.START_MOVE:
                StartMove();
                break;
            case GhostStates.MOVE:
                Move();
                break;
        }
    }

    public override void Idle()
    {
        if (isActive && (Time.time - idleEnterTime >= idleTime))
        {
            // Pick a small random direction to move slightly while in idle
            Vector2 randomDir = Random.insideUnitCircle.normalized;
            Vector2 velocity = randomDir * (speed * 0.5f); // slower speed while idle
            rigidBody.velocity = velocity;
        }
        else
        {
            currState = GhostStates.START_MOVE;
        }
    }

    public void StartMove()
    {
        moveStartTime = Time.time;

        Vector2 randomDir = Random.insideUnitCircle.normalized;
        movement = randomDir;
        moveDestination = transform.position + (Vector3)(randomDir * moveDistance);

        effectToApply = (EffectTypes)(Random.Range(0, (int)EffectTypes.NUM_EFFECTS));
        currState = GhostStates.MOVE;
    }

    public override void Move()
    {
        Vector2 direction;
        float targetSpeed = speed;

        if (Player != null && Vector3.Distance(transform.position, Player.transform.position) <= detectionRange)
        {
            // Chase the player
            direction = (Player.transform.position - transform.position).normalized;
            targetSpeed = chaseSpeed;

            // Turn red
            if (spriteRenderer != null)
            spriteRenderer.color = chaseColor;

            // Chase effect on
            if (snowEffect != null)
            snowEffect.isActive = true;
        }
        else
        {
            // Wander in chosen random direction
            direction = (moveDestination - transform.position).normalized;
            
            // Reset color
            if (spriteRenderer != null)
            spriteRenderer.color = defaultColor;

            // Chase effect off
            if (snowEffect != null)
            snowEffect.isActive = false;
        }

        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime * acceleration);
        rigidBody.velocity = direction * currentSpeed;

        if ((Vector2.Distance(transform.position, moveDestination) <= 0.1f) || (Time.time - moveStartTime >= maxMoveTime))
        {
            StartIdle();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Flashlight"))
        {
            StartCoroutine(DespawnWithEffect());
        }
    }

    private IEnumerator DespawnWithEffect()
    {
        // Play particle effect
        if (despawnParticles != null)
        {
            despawnParticles.transform.parent = null; // Detach from ghost
            despawnParticles.Play();
        }

        // // Optional: disable ghost visuals or collider here
        // GetComponent<SpriteRenderer>().enabled = false;
        // GetComponent<Collider2D>().enabled = false;
        // rigidBody.velocity = Vector2.zero;

        // Wait for particle duration
        yield return new WaitForSeconds(.1f); // particle duration

        Destroy(gameObject);
    }

    public override void Attack(PlayerBehavior player)
    {
        
    }

    public override void OnInteractWithFlashLight()
    {
        StartCoroutine(DespawnWithEffect());
    }
}
