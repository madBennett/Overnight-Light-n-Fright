using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MobGhostStates
{
    IDLE_WANDER,
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

    public float idleEnterTime;
    public float idleTime = 1f; // Wander time before chase
    public float speed = 1f;
    public float chaseSpeed = 2f;
    public float fleeSpeed = 2.5f;
    public float acceleration = 5f;
    public float detectionRange = 3f;
    public float fleeRange = 4f;

    public Rigidbody2D rigidBody;
    private GameObject player;
    private Vector3 moveDestination;

    // Cosmetic
    private SpriteRenderer spriteRenderer;
    private Color defaultColor = Color.white;
    private Color chaseColor = Color.red;
    private Color fleeColor = Color.blue;
    private MobShaderController shaderEffects;
    private bool chaseShaderActive = false;
    private bool isChasing = false;
    private bool isFleeing = false;
    public ParticleSystem despawnParticles;

    private void Start()
    {
        currState = MobGhostStates.IDLE_WANDER;
        idleEnterTime = Time.time;
        rigidBody = GetComponent<Rigidbody2D>();
        Player = GameObject.FindGameObjectWithTag("Player");
        EffectsManager = GameObject.FindGameObjectWithTag("EffectManager").GetComponent<EffectsManager>();
        player = Player;
        shaderEffects = FindObjectOfType<MobShaderController>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            defaultColor = spriteRenderer.color;
        }

        isActive = true;
    }

    private void Update()
    {
        if (!isActive) return;

        switch (currState)
        {
            case MobGhostStates.IDLE_WANDER:
                HandleWander();
                break;
            case MobGhostStates.CHASE:
                HandleChase();
                break;
            case MobGhostStates.FLEE:
                HandleFlee();
                break;
        }
    }

    private void HandleWander()
    {
        if (player == null) return;

        // Move in small random directions
        if (rigidBody.velocity.magnitude < 0.1f)
        {
            Vector2 randomDir = Random.insideUnitCircle.normalized;
            rigidBody.velocity = randomDir * speed;
        }

        // After idleTime seconds, start chasing
        if (Time.time - idleEnterTime > idleTime)
        {
            currState = MobGhostStates.CHASE;
        }

        // If player comes into range earlier, chase immediately
        if (Vector3.Distance(transform.position, player.transform.position) <= detectionRange)
        {
            currState = MobGhostStates.CHASE;
        }
    }

    private void HandleChase()
    {
        if (player == null) return;

        Vector2 direction = (player.transform.position - transform.position).normalized;
        float targetSpeed = chaseSpeed;

        // Set velocity towards player
        rigidBody.velocity = Vector2.Lerp(rigidBody.velocity, direction * targetSpeed, Time.deltaTime * acceleration);

        // Color to chase color
        if (spriteRenderer != null) spriteRenderer.color = chaseColor;

        if (!isChasing)
        {
            isChasing = true;
            if (shaderEffects != null && !chaseShaderActive)
            {
                chaseShaderActive = true;
                shaderEffects.AddChasingGhost();
            }
        }
    }

    private void HandleFlee()
    {
        if (player == null) return;

        Vector2 directionAway = (transform.position - player.transform.position).normalized;
        rigidBody.velocity = directionAway * fleeSpeed;

        if (spriteRenderer != null) spriteRenderer.color = fleeColor;

        // Once far enough, return to idle
        if (Vector3.Distance(transform.position, player.transform.position) >= fleeRange)
        {
            isFleeing = false;
            StartIdleWander();
        }
    }

    private void StartIdleWander()
    {
        idleEnterTime = Time.time;
        currState = MobGhostStates.IDLE_WANDER;
        rigidBody.velocity = Vector2.zero;
        if (spriteRenderer != null) spriteRenderer.color = defaultColor;

        if (shaderEffects != null && chaseShaderActive)
        {
            chaseShaderActive = false;
            shaderEffects.RemoveChasingGhost();
        }
        isChasing = false;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
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
        if (shaderEffects != null && chaseShaderActive)
        {
            chaseShaderActive = false;
            shaderEffects.RemoveChasingGhost();
        }

        if (despawnParticles != null)
        {
            ParticleSystem particles = Instantiate(despawnParticles, transform.position, Quaternion.identity);
            float duration = particles.main.duration + particles.main.startLifetime.constantMax;
            particles.Play();
            Destroy(particles.gameObject, duration);
        }

        yield return new WaitForSeconds(0.1f);

        if (onGhostDespawned != null)
        {
            onGhostDespawned(this);
        }

        Destroy(gameObject);
    }

    public void Attack(PlayerBehavior player) { }
}
