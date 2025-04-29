using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MobGhostStates
{
    IDLE_WANDER,
    START_CHASE,
    CHASE,
    FLEE
}

public class MobGhostBehavior : MonoBehaviour
{
    public delegate void GhostDespawned(MobGhostBehavior ghost);
    public event GhostDespawned onGhostDespawned;

    public MobGhostStates currState;
    public bool isActive = false;
    public GameObject Player;
    public EffectsManager EffectsManager;

    // variables for idle wander state
    public float idleEnterTime;
    public float idleTime = 1f;

    // variables for start chase state
    public Vector2 movement;
    public EffectTypes effectToApply;
    public float moveStartTime;

    // variables for chase state
    public float speed = 1f;
    public float maxMoveTime = 3f;
    public Rigidbody2D rigidBody;

    private Vector3 moveDestination;
    public float moveDistance = 0.5f;

    private float currentSpeed;
    public float chaseSpeed = 2f;
    public float acceleration = 5f;

    private GameObject player;
    public float detectionRange = 3f;
    private bool isChasing = false;

    // variables for flee state
    public float fleeSpeed = 2.5f;
    public float fleeRange = 4f;
    private bool isFleeing = false;
    
    // cosmetic
    public ParticleSystem despawnParticles;
    private MobShaderController shaderEffects;
    private SpriteRenderer spriteRenderer;
    private Color defaultColor = Color.white;
    private Color chaseColor = Color.red;
    private Color fleeColor = Color.blue;
    private bool chaseShaderActive = false;

    private void Start()
    {
        //set default values
        currState = MobGhostStates.IDLE_WANDER;
        idleEnterTime = Time.time;
        effectToApply = EffectTypes.VISUAL_DISTORTION;
        rigidBody = GetComponent<Rigidbody2D>();
        Player = GameObject.FindGameObjectWithTag("Player");
        EffectsManager = GameObject.FindGameObjectWithTag("EffectManager").GetComponent<EffectsManager>();

        isActive = true;
        player = GameObject.FindGameObjectWithTag("Player");
        shaderEffects = FindObjectOfType<MobShaderController>();
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
        if (!isActive) return;

        switch (currState)
        {
            case MobGhostStates.IDLE_WANDER:
                Wander();

                // Go to CHASE state after 1 second
                if (Time.time - idleEnterTime > idleTime)
                    currState = MobGhostStates.START_CHASE;
                break;

            case MobGhostStates.START_CHASE:
                StartMove();
                break;

            case MobGhostStates.CHASE:
                Move();
                break;

            case MobGhostStates.FLEE:
                Flee();
                break;
        }
    }

    private void StartMove()
    {
        moveStartTime = Time.time;

        Vector2 randomDir = Random.insideUnitCircle.normalized;
        movement = randomDir;
        moveDestination = transform.position + (Vector3)(randomDir * moveDistance);

        currState = MobGhostStates.CHASE;
    }

    public void Move()
    {
        Vector2 direction;
        float targetSpeed = speed;
        bool withinRange = player != null && Vector3.Distance(transform.position, player.transform.position) <= detectionRange;

        if (withinRange)
        {
            direction = (player.transform.position - transform.position).normalized;
            targetSpeed = chaseSpeed;

            // start chasing 
            if (!isChasing)
            {
                isChasing = true;

                if (shaderEffects != null && !chaseShaderActive)
                {
                    chaseShaderActive = true;
                    shaderEffects.AddChasingGhost();
                }
            }
            // make ghost chase color
            if (spriteRenderer != null) spriteRenderer.color = chaseColor;
        }
        else
        {
            direction = (moveDestination - transform.position).normalized;
            targetSpeed = speed;

            // stop chasing
            if (isChasing)
            {
                isChasing = false;

                if (shaderEffects != null && chaseShaderActive)
                {
                    chaseShaderActive = false;
                    shaderEffects.RemoveChasingGhost();
                }
            }
            // make ghost default color
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //if collision with a player attack
            Attack(Player.GetComponent<PlayerBehavior>());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Flashlight"))
        {
            StartCoroutine(DespawnWithEffect());
        }
        else if (collision.gameObject.CompareTag("Scare"))
        {
            TriggerFlee();
        }
    }

    private IEnumerator DespawnWithEffect()
    {
        // disable shader chase effect
        if (shaderEffects != null && chaseShaderActive)
        {
            chaseShaderActive = false;
            shaderEffects.RemoveChasingGhost();
        }

        // play despawn particle effect
        if (despawnParticles != null)
        {
            ParticleSystem particles = Instantiate(despawnParticles, transform.position, Quaternion.identity);
            float duration = particles.main.duration + particles.main.startLifetime.constantMax;
            particles.Play(); // play particle
            Destroy(particles.gameObject, duration); // destroy particle
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

    public void Wander()
    {
        // Pick a small random direction to move slightly while in idle
        Vector2 randomDir = Random.insideUnitCircle.normalized;
        Vector2 velocity = randomDir * (speed);
        rigidBody.velocity = velocity;
    }

    public void StartIdle()
    {
        idleEnterTime = Time.time;
        rigidBody.velocity = Vector2.zero;
        currState = MobGhostStates.IDLE_WANDER;
            
        if (spriteRenderer != null) spriteRenderer.color = defaultColor; // Reset ghost color
    }

    private void TriggerFlee()
    {
        if (!isFleeing)
        {
            isFleeing = true;
            currState = MobGhostStates.FLEE;
            rigidBody.velocity = Vector2.zero;
        }
    }

    private void Flee()
    {
        if (player == null) return;

        Vector2 directionAway = (transform.position - player.transform.position).normalized;
        rigidBody.velocity = directionAway * fleeSpeed;

        // Turn blue while fleeing
        if (spriteRenderer != null) spriteRenderer.color = fleeColor;

        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance >= fleeRange)
        {
            isFleeing = false;
            StartIdle(); // Return to idle before chase resumes
        }
    }

    // public void Idle() { }
    public void Attack(PlayerBehavior player) { }
    // public void OnInteractWithFlashLight() { }
}
