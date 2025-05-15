using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : AbstractGhostBehavior
{

    private Vector2 vel;
    private SurvivalTimer timeLeftCounter;

    private GameObject timeIncreaseIndicator;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        isActive = true;
        speed = 5f;
        //vel = new Vector2(0, 1);


    }


    void FixedUpdate()
    {

        HandleMove(vel, 1f);
        
    }

    public void setVelAndGiveTimer(Vector2 newVel, SurvivalTimer timerReference, GameObject timeIndicator)
    {
        timeIncreaseIndicator = timeIndicator;
        vel.x = newVel.x;
        vel.y = newVel.y;
        timeLeftCounter = timerReference;

    }

    public void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            if (true) //REPLACE IF NECESSARY with conditions that prevent ghost from hitting (post-hit immunity frames or something)
            {
                EffectsManager.ApplyEffect(effectToApply);

                timeLeftCounter.addTime(5f);

                Vector3 abovePlayer = Player.transform.position;
                abovePlayer.y = abovePlayer.y + 1;
                Object.Instantiate(timeIncreaseIndicator, abovePlayer, Quaternion.identity);


            }
        }
        else if (collision.gameObject.tag == "Wall")
        {
            
        }

        GameObject.Destroy(gameObject);
    }
}
