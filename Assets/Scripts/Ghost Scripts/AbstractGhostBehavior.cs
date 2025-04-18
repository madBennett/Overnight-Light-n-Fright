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
    DEATH,//haha died twice
    NUM_STATES
}

public abstract class AbstractGhostBehavior : MonoBehaviour
{
    public GhostStates currState;
    public bool isActive = false; //bool to determine if the ghost should be moving or not
    public GameObject Player;

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
    public Rigidbody2D rigidBody;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        //set default values
        currState = GhostStates.IDLE;
        idleEnterTime = Time.time;
        effectToApply = EffectTypes.VISUAL_DISTORTION;
        rigidBody = GetComponent<Rigidbody2D>();
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            //if collision with a player attack
            Attack(Player.GetComponent<PlayerBehavior>());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Flashlight")
        {
            //on collision with flashlight
            OnInteractWithFlashLight();
        }
    }

    public virtual void StartIdle()
    {
        idleEnterTime = Time.time;
        rigidBody.velocity = Vector2.zero;
        currState = GhostStates.IDLE;
    }

    public abstract void Idle();

    public abstract void Move();

    public abstract void Attack(PlayerBehavior player);

    public abstract void OnInteractWithFlashLight();
}
