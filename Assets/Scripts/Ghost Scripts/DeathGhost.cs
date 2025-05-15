using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum DeathStates
{
    WAIT,
    CHASE,
    STARTLUNGE,
    LUNGE,
    NUM_STATES
}

public class DeathGhost : AbstractGhostBehavior
{
    public DeathStates state;

    private GameManager game;

    private Vector2 velocity;
    private Vector2 targetVel;

    private Vector2 playerPos;
    private Vector2 ourPos;

    private Vector2 us2Player;
    

    public float lungeStartRange = 5f;
    public float lungeCooldown = 5f;
    public float lungeWait = 0.75f;
    public float lungeTime = 0.5f;
    public float lungeSpeed = 7.5f;
    private float lastLungeTime = 0f;
    private float phaseStartTime = 0f;
    private Vector2 lungeDir;

    public float acceleration = 5f;

    protected override void Start()
    {
        base.Start();
        isActive = true;
        state = DeathStates.WAIT;

        game = GameObject.Find("GameManager").GetComponent<GameManager>();
    }


    void FixedUpdate()
    {
        playerPos.x = Player.transform.position.x;
        playerPos.y = Player.transform.position.y;

        ourPos.x = transform.position.x;
        ourPos.y = transform.position.y;

        us2Player = playerPos - ourPos;

        switch(state)
        {
            case DeathStates.WAIT:
                //do nothing until the gameManager says time is up

                if (game.YourTimeIsUp)
                {
                    Debug.Log("Began the chase!");
                    state = DeathStates.CHASE;
                }
                break;

            case DeathStates.CHASE:
                targetVel = us2Player * speed;

                if ((us2Player.magnitude <= lungeStartRange) && Time.time >= lastLungeTime + lungeCooldown)
                {
                    Debug.Log("Readying for a lunge!");

                    phaseStartTime = Time.time;
                    state = DeathStates.STARTLUNGE;
                }
                break;

            case DeathStates.STARTLUNGE:
                targetVel = Vector2.zero;

                if (Time.time >= phaseStartTime + lungeWait)
                {
                    Debug.Log("Lunging!");
                    lastLungeTime = Time.time;

                    phaseStartTime = Time.time;
                    lungeDir = us2Player.normalized;
                    velocity = lungeDir * lungeSpeed;
                    state = DeathStates.LUNGE;
                }
                break;

            case DeathStates.LUNGE:
                targetVel = lungeDir * lungeSpeed;
                if (Time.time >= phaseStartTime + lungeTime)
                {
                    Debug.Log("Done lunging!");
                    state = DeathStates.CHASE;
                }
                break;

        }

        Vector2 deltaV = targetVel - velocity;
        Vector2 accel = (deltaV.magnitude >= 0.1) ? deltaV.normalized * acceleration : Vector2.zero;

        velocity = velocity + (accel * Time.deltaTime);


        //Whatever Velocity is, move it
            HandleMove(velocity, 1f); //we use 1 because speed is used in the making of the velocity vector.
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && !LevelLoader.Instance.currentlyLoading) //dont kill while walking through a door
        {

            GameOver();

        }
    }

    public void GameOver()
    {
        //LevelLoader disabled because I felt it was more appropriate if it was more sudden
        
        //if (LevelLoader.Instance != null)
        //{
            //LevelLoader.Instance.LoadScene("GameLoss");
        //}
        //else
        //{
            SceneManager.LoadScene("GameLoss");
        //}
    }
}
