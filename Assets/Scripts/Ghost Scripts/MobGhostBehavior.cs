using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobGhostBehavior : AbstractGhostBehavior
{
    public float moveDistance = 1f;
    public float detectionRange = 5f;
    private GameObject player;
    private Vector3 moveDestination;
    private float idleWanderCooldown = 0f;
    public ParticleSystem despawnParticles;
    private SnowEffectController snowEffect;

    protected override void Start()
    {
        base.Start();
        isActive = true;
        player = GameObject.FindGameObjectWithTag("Player");
        snowEffect = Camera.main.GetComponent<SnowEffectController>();
    }

    private void Update()
    {
        if (currState == GhostStates.IDLE)
        {
            // Wander while idle
            if (isActive && Time.time >= idleWanderCooldown)
            {
                WanderWhileIdle();
                idleWanderCooldown = Time.time + idleTime;
            }
        }

        base.Update(); // Run the base state machine logic
    }

    private void WanderWhileIdle()
    {
        // Pick a small random direction to move slightly while in idle
        Vector2 randomDir = Random.insideUnitCircle.normalized;
        Vector2 velocity = randomDir * (speed * 0.5f); // slower speed while idle
        rigidBody.velocity = velocity;
    }

    public override void StartMove()
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

        if (player != null && Vector3.Distance(transform.position, player.transform.position) <= detectionRange)
        {
            // Chase the player
            direction = (player.transform.position - transform.position).normalized;

            if (snowEffect != null)
            snowEffect.isActive = true;
        }
        else
        {
            // Wander in chosen random direction
            direction = (moveDestination - transform.position).normalized;
            
            if (snowEffect != null)
            snowEffect.isActive = false;
        }

        rigidBody.velocity = direction * speed;

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

        // Wait for particle duration
        yield return new WaitForSeconds(.1f); // particle duration

        Destroy(gameObject);
    }

    public override void Attack(PlayerBehavior player)
    {
        
    }

    public override void StartRun()
    {

    }

    public override void Run()
    {

    }
}
