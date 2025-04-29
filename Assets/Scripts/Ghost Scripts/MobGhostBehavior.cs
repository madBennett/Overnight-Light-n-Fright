using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobGhostBehavior : MonoBehaviour
{
    public delegate void GhostDespawned(MobGhostBehavior ghost);
    public event GhostDespawned onGhostDespawned;

    public GhostStates currState;
    public bool isActive = false; //bool to determine if the ghost should be moving or not
    public GameObject Player;
    public EffectsManager EffectsManager;

    // variables for idle state
    public float idleEnterTime;
    public float idleTime = 1f;

    // variables for start move state
    public Vector2 movement;
    public EffectTypes effectToApply;
    public float moveStartTime;

    // Variables for move
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

    public ParticleSystem despawnParticles;
    private MobShaderController shaderEffects;

    private SpriteRenderer spriteRenderer;
    private Color defaultColor = Color.white;
    private Color chaseColor = Color.red;

    private bool chaseShaderActive = false;

    private void Start()
    {
        //set default values
        currState = GhostStates.IDLE;
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
        if (currState == GhostStates.IDLE)
        {
            // Wander while idle
            if (isActive)
            {
                Wander();
            }
        }

        switch (currState)
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

    private void StartMove()
    {
        moveStartTime = Time.time;

        Vector2 randomDir = Random.insideUnitCircle.normalized;
        movement = randomDir;
        moveDestination = transform.position + (Vector3)(randomDir * moveDistance);

        currState = GhostStates.MOVE;
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
        currState = GhostStates.IDLE;
    }

    public void Idle() { }
    public void Attack(PlayerBehavior player) { }
    public void OnInteractWithFlashLight() { }
}
