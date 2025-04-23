using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobGhostBehavior : AbstractGhostBehavior
{
    public delegate void GhostDespawned(MobGhostBehavior ghost);
    public event GhostDespawned onGhostDespawned;

    private Vector3 moveDestination;
    public float moveDistance = 0.5f;

    private float currentSpeed;
    public float chaseSpeed = 2f;
    public float acceleration = 5f;

    private GameObject player;
    public float detectionRange = 3f;
    private bool isChasing = false;

    public ParticleSystem despawnParticles;
    private SnowEffectController snowEffect;

    private SpriteRenderer spriteRenderer;
    private Color defaultColor = Color.white;
    private Color chaseColor = Color.red;

    protected override void Start()
    {
        base.Start();
        isActive = true;
        player = GameObject.FindGameObjectWithTag("Player");
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
        if (currState == GhostStates.IDLE)
        {
            // Wander while idle
            if (isActive)
            {
                Wander();
            }
        }

        switch(currState)
       {
           case GhostStates.IDLE:
               //if the ghost is not active do not allow out of the idle state
               //in addition if the ghost is active but enters the idle state is must stay in it for x time
               if (isActive)
               {
                   currState = GhostStates.START_MOVE;
               }
               break;
           case GhostStates.START_MOVE:
               StartMove();
               break;
           case GhostStates.MOVE:
               Move();
               break;
       }
    }

    private void Wander()
    {
        // Pick a small random direction to move slightly while in idle
        Vector2 randomDir = Random.insideUnitCircle.normalized;
        Vector2 velocity = randomDir * (speed);
        rigidBody.velocity = velocity;
    }

    private void StartMove()
    {
        moveStartTime = Time.time;

        Vector2 randomDir = Random.insideUnitCircle.normalized;
        movement = randomDir;
        moveDestination = transform.position + (Vector3)(randomDir * moveDistance);

        //effectToApply = (EffectTypes)(Random.Range(0, (int)EffectTypes.NUM_EFFECTS));
        currState = GhostStates.MOVE;
    }

    public override void Move()
    {
        Vector2 direction;
        float targetSpeed = speed;
        bool withinRange = player != null && Vector3.Distance(transform.position, player.transform.position) <= detectionRange;

        if (withinRange)
        {
            // Chase the player
            direction = (player.transform.position - transform.position).normalized;
            targetSpeed = chaseSpeed;

            if (!isChasing) 
            {
                isChasing = true;
                if (snowEffect != null) snowEffect.AddChasingGhost();
            }

            if (spriteRenderer != null) spriteRenderer.color = chaseColor;
        } 
        else 
        {
            direction = (moveDestination - transform.position).normalized;
            targetSpeed = speed;

            if (isChasing)
            {
                isChasing = false;
                if (snowEffect != null) snowEffect.RemoveChasingGhost(); 
            }

            if (spriteRenderer != null) spriteRenderer.color = defaultColor;
        }

        // Chase logic
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime * acceleration);
        rigidBody.velocity = direction * currentSpeed;

        // if out of range idle
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
        // disable shader chase effect
        if (isChasing && snowEffect != null)
        {
            snowEffect.RemoveChasingGhost();
        }

        // play despawn particle effect
        if (despawnParticles != null)
        {
            ParticleSystem particles = Instantiate(despawnParticles, transform.position, Quaternion.identity);
            float duration = particles.main.duration + particles.main.startLifetime.constantMax;
            particles.Play(); // play particle
            Destroy(particles.gameObject, duration);// destroy particle
        }

        // wait for particle duration
        yield return new WaitForSeconds(.1f);

        // notify spawner
        if (onGhostDespawned != null)
        {
            onGhostDespawned(this);
        }

        // destroy the ghost object
        Destroy(gameObject);
    }

    public override void Idle() {}
    public override void Attack(PlayerBehavior player) {}
    public override void OnInteractWithFlashLight() {}
}
