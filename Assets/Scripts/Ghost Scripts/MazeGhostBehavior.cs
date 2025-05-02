using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MazeGhostStates
{
    IDLE,
    WANDER,
    HUNT,
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

    //color varibles
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private List<Color> colorMap = new List<Color>() { new Color (0,0,0,255), new Color(255,0,0,128), new Color(0,255,0,128), new Color(0,0,255,128), new Color(0,255,255,128)};

    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
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
                        StartMoveStates(MazeGhostStates.HUNT);
                    }
                    else
                    {
                        StartMoveStates(MazeGhostStates.WANDER);
                    }
                }
                break;
            case MazeGhostStates.WANDER:
            case MazeGhostStates.SCARED:
            case  MazeGhostStates.HUNT:
                Move();
                break;
            case MazeGhostStates.HIDE:
                //if player in range attack else if timer up get out of wall and idle
                break;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {

        if ((collision.gameObject.tag == "Player") && (currState != MazeGhostStates.SCARED))
        {
            //if collision with a player attack
            EffectsManager.ApplyEffect(effectToApply);
        }
        else if ((collision.gameObject.tag == "Wall") && (currState == MazeGhostStates.SCARED))
        {
            StartHide(collision.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Flashlight")
        {
            //on collision with flashlight
            //scared animation??

            //Run
            StartMoveStates(MazeGhostStates.SCARED);
        }
    }

    private void StartIdle()
    {
        idleEnterTime = Time.time;
        rigidBody.velocity = Vector2.zero;
        currState = MazeGhostStates.IDLE;

        spriteRenderer.color = colorMap[0];
    }

    private void StartMoveStates(MazeGhostStates newState)
    {
        //select movment destination and move towards it based on state
        switch (newState)
        {
            case MazeGhostStates.SCARED:
                //choose a direction that is opposite the player
                movement = -1 * (Player.transform.position - transform.position).normalized;
                break;
            case MazeGhostStates.HUNT:
                //choose a direction towards the player
                movement = (Player.transform.position - transform.position).normalized;
                //choose effect
                effectToApply = (EffectTypes)(Random.Range(1, (int)EffectTypes.NUM_EFFECTS));
                spriteRenderer.color = colorMap[(int)effectToApply];
                break;
            case MazeGhostStates.WANDER:
                //choose a random cardinal direction
                int numDir = 4;
                Vector2[] cardDir = {new Vector2(-1,0), new Vector2(1, 0), new Vector2(0, -1), new Vector2(0, 1)};
                int randIndex = Random.Range(0, numDir-1);
                movement = cardDir[randIndex];
                break;
        }

        //set start move time
        moveStartTime = Time.time;

        //set new state
        currState = newState;
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

    private void StartHide(GameObject wall)
    {
        //
    }
}
