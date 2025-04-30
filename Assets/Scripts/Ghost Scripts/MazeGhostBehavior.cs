using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MazeGhostStates
{
    IDLE,
    WANDER,
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
    public MazeGhostStates currState;

    //varibles for idle state
    public float idleEnterTime;
    public float idleTime = 1f;

    //varibles for move
    private float moveStartTime;
    private float maxMoveTime = 3f;
    [SerializeField] private float minMoveDistance = 1f;

    //Scared Varibles
    [SerializeField] private float hideOdds = 0.5f;


    protected override void Start()
    {
        base.Start();
    }
    
    public void Update()
    {
        switch (currState)
        {
            case  MazeGhostStates.IDLE:
                //if the ghost is not active do not allow out of the idle state
                //in addition if the ghost is active but enters the idle state is must stay in it for x time
                if (isActive && (Time.time - idleEnterTime >= idleTime))
                {
                    //if player is in range move
                    if (Vector2.Distance(transform.position, Player.transform.position) < minMoveDistance)
                    {
                        StartMove();
                    }
                    else
                    {
                        Wander();
                    }
                }
                break;
            case  MazeGhostStates.MOVE:
                Move();
                break;
            case MazeGhostStates.HIDE:
                //if player in range attack else if timer up get out of wall and idle
                break;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            //if collision with a player attack
            EffectsManager.ApplyEffect(effectToApply);
        }
        //if collide with power up stay and camp
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Flashlight")
        {
            //on collision with flashlight
            currState = MazeGhostStates.SCARED;
            //scared animation??
            if (Random.Range(0f, 1f) < hideOdds)
            {
                //Hide
            }
            else
            {
                //Run
                StartRun();
            }
        }
    }

    private void StartIdle()
    {
        idleEnterTime = Time.time;
        rigidBody.velocity = Vector2.zero;
        currState = MazeGhostStates.IDLE;
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

        //choose a direction
        movement = (Player.transform.position - transform.position).normalized;

        //choose effect
        effectToApply = (EffectTypes)(Random.Range(1, (int)EffectTypes.NUM_EFFECTS));

        //change State to move
        currState =  MazeGhostStates.MOVE;
    }

    private void StartRun()
    {
        //select movment destination and move towards it

        //choose a direction that is opposite the player
        movement = -1 * (Player.transform.position - transform.position).normalized;

        //change State to move
        currState = MazeGhostStates.MOVE;

    }

    private void Move()
    {
        //check if exceeded move time
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
}
