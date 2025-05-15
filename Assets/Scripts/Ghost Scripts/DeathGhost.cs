using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum DeathStates
{
    WAIT,
    CHASE,
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

    private float acceleration = 5f;

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
                    state = DeathStates.CHASE;
                }
                break;

            case DeathStates.CHASE:
                velocity = us2Player;
                break;

        }

        //Vector2 deltaV = targetVel - velocity;
        //Vector2 accel = (deltaV.magnitude >= 0.1) ? deltaV.normalized * speed : Vector2.zero;

        //velocity = velocity + (accel * Time.deltaTime);


        //Whatever Velocity is, move it
        //if (state != BulletGhostStates.DASH) //except if we're dashing, dashing uses separate rules
        //{
            HandleMove(velocity, 1f); //we use 1 because speed is used in the making of the velocity vector.
        //}
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
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
