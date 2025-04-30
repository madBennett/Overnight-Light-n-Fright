using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MazeGhostStates
{
    IDLE,
    WANDER,
    START_MOVE,
    MOVE,
    CAMP,
    SNEAK,
    SCARED,
    HIDE,
    RUN,
    NUM_STATES
}

public class MazeGhostBehavior : AbstractGhostBehavior
{

    //varibles for idle state
    public float idleEnterTime;
    public float idleTime = 1f;

    private float moveStartTime;
    private float maxMoveTime = 3f;

    public MazeGhostStates currState;
    protected override void Start()
    {
        base.Start();
    }
    
    public void Update()
    {
        switch (currState)
        {
            case  MazeGhostStates.IDLE:
                Idle();
                break;
            case  MazeGhostStates.START_MOVE:
                StartMove();
                break;
            case  MazeGhostStates.MOVE:
                Move();
                break;
            case  MazeGhostStates.RUN:
                Run();
                break;
        }
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

    public virtual void Wander()
    {
        // Pick a small random direction to move slightly while in idle
        Vector2 randomDir = Random.insideUnitCircle.normalized;
        Vector2 velocity = randomDir * (speed);
        rigidBody.velocity = velocity;
    }

    public virtual void StartIdle()
    {
        idleEnterTime = Time.time;
        rigidBody.velocity = Vector2.zero;
    }

    public void StartMove()
    {
        moveStartTime = Time.time;

        //choose a direction
        movement = new Vector2(0,1);

        //choose effect
        effectToApply = (EffectTypes)(Random.Range(1, (int)EffectTypes.NUM_EFFECTS));

        //change State to move
        currState =  MazeGhostStates.MOVE;
    }

    public void Move()
    {
        //check if has reached the position
        if (Time.time - moveStartTime >= maxMoveTime)
        {
            StartIdle();
        }
        else
        {
            //Move in choosen direction move through rigid body
            rigidBody.velocity = movement * speed;
        }

    }

    public void Idle()
    {
        //if the ghost is not active do not allow out of the idle state
        //in addition if the ghost is active but enters the idle state is must stay in it for x time
        if (isActive && (Time.time - idleEnterTime >= idleTime))
        {
            currState =  MazeGhostStates.START_MOVE;
        }
    }

    public void Attack(PlayerBehavior player)
    {
        //
        EffectsManager.ApplyEffect(effectToApply);
    }

    public void OnInteractWithFlashLight()
    {
        currState =  MazeGhostStates.RUN;
        StartRun();
    }

    public void StartRun()
    {
        //select movment destination and move towards it
    }

    public void Run()
    {
        //If the ghost is in contact with the flashlight move in the oppistae dircetion of the player
    }
}
