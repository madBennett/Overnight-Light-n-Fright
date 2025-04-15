using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GhostStates
{
    IDLE,
    START_MOVE,
    MOVE,
    ATTACK,
    RUN,
    NUM_STATES
}

public enum EffectTypes
{
    VISUAL_DISTORTION,
    REVERSE_CONTROLS,
    STUN,
    DAMAGE,
    TELEPORT,
    NUM_EFFECTS
}

public abstract class AbstractGhostBehavior : MonoBehaviour
{
    public GhostStates currState;
    public bool isActive = false; //bool to determine if the ghost should be moving or not

    //varibles for idle state
    public float idleEnterTime;
    public float idleTime = 1f;

    //varibles for start move state
    public Vector2 movement;
    public EffectTypes effectToApply;
    public float moveStartTime;

    //Varibles for move
    public float speed = 1f;
    public float maxMoveTime = 3f;

    //Varibles for Run state
    public Vector3 playerPos;
    public Rigidbody2D rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        //set default values
        currState = GhostStates.IDLE;
        idleEnterTime = Time.time;
        effectToApply = EffectTypes.VISUAL_DISTORTION;
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        switch(currState)
        {
            case GhostStates.IDLE:
                //if the ghost is not active do not allow out of the idle state
                //in addition if the ghost is active but enters the idle state is must stay in it for x time
                if (isActive && (Time.time - idleEnterTime >= idleTime))
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
            case GhostStates.RUN:
                Run();
                break;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            //if collision with a player attack
            PlayerBehavior player = collision.gameObject.GetComponent<PlayerBehavior>();
            Attack(player);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Flashlight")
        {
            //on collision with flashlight
            StartRun();
            currState = GhostStates.RUN;
            playerPos = collision.gameObject.transform.position;
        }
    }

    public virtual void StartIdle()
    {
        idleEnterTime = Time.time;
        rigidBody.velocity = Vector2.zero;
        currState = GhostStates.IDLE;
    }

    public abstract void StartMove();

    public abstract void Move();

    public abstract void Attack(PlayerBehavior player);

    public abstract void StartRun();

    public abstract void Run();
}
