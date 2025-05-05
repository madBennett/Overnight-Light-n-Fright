using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MazeGhostStates
{
    IDLE,
    WANDER,
    HUNT,
    SCARED,
    HIDE,
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

    //Scared Varibles
    [SerializeField] private float hideOdds = 0.05f;
    [SerializeField] private float hideCoolDown = 2f;
    private float lastHideTime = 0f;

    //hunt
    [SerializeField] private float detectionRange = 5f;

    //color varibles
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private List<Color> colorMap = new List<Color>() 
        {  
            Color.white,    //default
            Color.yellow,   //Visual Distortion
            Color.blue,     //Reverse Controls
            Color.green,    //Stun
            Color.red       //Damage
        };

    [SerializeField] private Collider2D collider;

    protected override void Start()
    {
        base.Start();
        isActive = true;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = colorMap[0];
        speed = 3;

        collider = GetComponent<Collider2D>();
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
                    //if player is in range hunt
                    if (CheckPlayerInRange())
                    {
                        StartHunt();
                    }
                    else
                    {
                        StartWander();
                    }
                }
                break;
            case MazeGhostStates.WANDER:
                //if player is in range hunt
                if (CheckPlayerInRange())
                {
                    StartHunt();
                }
                Move();
                break;
            case MazeGhostStates.SCARED:
                movement = -1 * (Player.transform.position - transform.position).normalized;
                Move();
                break;
            case  MazeGhostStates.HUNT:
                movement = (Player.transform.position - transform.position).normalized;
                Move();
                break;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            if ((currState != MazeGhostStates.SCARED) && (currState != MazeGhostStates.HIDE))
            {
                //if collision with a player attack
                EffectsManager.ApplyEffect(effectToApply);
            }
        }
        else if (collision.gameObject.tag == "Wall")
        {
            if ((currState == MazeGhostStates.SCARED) 
                || (currState == MazeGhostStates.WANDER && (Random.Range(0f, 1f) <= hideOdds)))
            {
                //if scared or if wander dd scucced then hide
                StartHide(collision.gameObject);
            }
            else if (currState == MazeGhostStates.WANDER)
            {
                //if it wall in wander reverse the movement
                movement *= -1;
            }
        }
    }

    private bool CheckPlayerInRange()
    {
        return (Vector2.Distance(transform.position, Player.transform.position) < detectionRange);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Flashlight")
        {
            //on collision with flashlight
            if (currState != MazeGhostStates.HIDE)
            {
                //scared animation??
                //Run
                StartScared();
            }
            else if (currState != MazeGhostStates.SCARED)
            {
                Destroy(gameObject);
            }
        }
    }

    private void StartIdle()
    {
        idleEnterTime = Time.time;
        rigidBody.velocity = Vector2.zero;
        currState = MazeGhostStates.IDLE;

        spriteRenderer.color = colorMap[0];
    }

    private void StartHunt()
    {
        //choose effect
        effectToApply = (EffectTypes)(Random.Range(1, (int)EffectTypes.NUM_EFFECTS - 1));
        spriteRenderer.color = colorMap[(int)effectToApply];//todo fix sometimes wrong color

        currState = MazeGhostStates.HUNT;
    }

    private void StartWander()
    {
        //choose a random cardinal direction
        int numDir = 4;
        Vector2[] cardDir = 
            { 
                Vector2.left,
                Vector2.right,
                Vector2.down,
                Vector2.up
            };
        int randIndex = Random.Range(0, numDir - 1);
        movement = cardDir[randIndex];

        currState = MazeGhostStates.WANDER;
        moveStartTime = Time.time;
    }

    private void StartScared()
    {
        //

        currState = MazeGhostStates.SCARED;
        moveStartTime = Time.time;
    }

    private void Move()
    {
        //check if exceeded move time
        if (((currState == MazeGhostStates.HUNT) 
                && (Vector2.Distance(transform.position, Player.transform.position) > detectionRange))
            || ((currState == MazeGhostStates.WANDER || currState == MazeGhostStates.SCARED) 
                && (Time.time - moveStartTime >= maxMoveTime)))
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
        if (Time.time - lastHideTime >= hideCoolDown)
        {
            // play particle effect

            //turn off sprite render
            spriteRenderer.enabled = false;
            rigidBody.velocity = Vector2.zero;
            collider.enabled = false;

            //move pos
            transform.position = wall.transform.position;

            //call start hide on the wall
            wall.GetComponent<MazeWallBehavior>().StartHaunt(this);

            currState = MazeGhostStates.HIDE;

        }
    }

    public void EndHide(bool hitPlayer)
    {
        transform.position = Player.transform.position;
        spriteRenderer.enabled = true;
        collider.enabled = true;
        lastHideTime = Time.time;

        if (hitPlayer)
        {
            StartHunt();
        }
        else
        {
            StartIdle();
        }
    }
}
