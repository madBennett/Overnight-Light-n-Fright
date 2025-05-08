using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletGhostStates
{
    CHASE,
    SELECTATTACK,
    FIRE,
    PREPAREDASH,
    DASH,
    COOLDOWN,
    NUM_STATES
}

public class BulletGhost : AbstractGhostBehavior
{

    public BulletGhostStates state;

    private SurvivalTimer timeLeftCounter;


    protected override void Start()
    {
        base.Start();
        isActive = true;
        state = BulletGhostStates.CHASE;

        if (GameObject.Find("Survive Timer") == null)
        {
            Debug.Log("No survival timer found! This ghost doesn't work without one!");
            return;
        }

        timeLeftCounter = GameObject.Find("Survive Timer").GetComponent<SurvivalTimer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (state)
        {
            case BulletGhostStates.CHASE:
                //chasing behavior (default)
                //advance towards player (potentially faster if player is farther)
                //after a semi-random length of time, transition to SELECTATTACK
                break;

            case BulletGhostStates.SELECTATTACK:
                //attack selection behavior
                //choose dash or fire (multiple fire patterns? either way same ghost state, probably)
                //then transition to appropriate attack state immediately
                break;

            case BulletGhostStates.PREPAREDASH:
                //dash preparation behavior
                //pick a direction (possibly where the player is, maybe leading a little), stop still for a second. Possibly spawn some visual indication.
                //then transition to DASH
                break;

            case BulletGhostStates.DASH:
                //dashing behavior
                //dash in decided upon direction at a particular speed for a particular distance
                //then transition to COOLDOWN
                break;

            case BulletGhostStates.FIRE:
                //firing bevahor
                //let loose A Bullet (will probably complexify later)
                //stand mostly still while firing (slow down to a stop)
                //when finished, transition to COOLDOWN
                break;

            case BulletGhostStates.COOLDOWN:
                //post-attack cooldown behavior
                //either shy away from player or move towards them very slowly
                //maybe become a bit transparent in this form, maybe be unable to hurt
                //after a while, transition back to CHASE

                break;

        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            if (true) //REPLACE IF NECESSARY with conditions that prevent ghost from hitting (post-hit immunity frames or something)
            {
                EffectsManager.ApplyEffect(effectToApply);

                timeLeftCounter.addTime(10f);
            }
        }
        else if (collision.gameObject.tag == "Wall")
        {
            
        }
    }
}
