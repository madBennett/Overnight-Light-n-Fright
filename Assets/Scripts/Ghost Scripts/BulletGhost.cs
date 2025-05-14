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

    public GameObject BulletPrefab;

    private SurvivalTimer timeLeftCounter;

    private Vector2 velocity;
    private Vector2 targetVel;

    private Vector2 playerPos;
    private Vector2 ourPos;

    private Vector2 us2Player;

    //shamelessly stolen from mob ghost
    public delegate void GhostDespawned(BulletGhost ghost);
    public event GhostDespawned onGhostDespawned;
    public ParticleSystem despawnParticles;

    public float ChaseTimeLowerBound = 5f;
    public float ChaseTimeUpperBound = 10f;
    public float ThinkingTime = 1f;
    public float DashPrepTime = 1f;
    public float DashTime = 0.5f;
    public float fireTime = 1.5f;
    public float CooldownTime = 1f;

    public int numDashes = 3;
    private int dashesLeft;
    public float DashSpeed = 20f;

    public float acceleration = 5f;

    private Vector2 dashDirection;

    public float bulletCooldown = 0.25f;
    public float timeLastBulletWasShot = 0f;
    public float bulletSpread = 30f;
    public float fireStrength = 15f;

    private float phaseLength;

    private float phaseStartTime;


    protected override void Start()
    {
        base.Start();
        isActive = true;
        state = BulletGhostStates.CHASE;

        phaseStartTime = Time.time;
        phaseLength = Random.Range(ChaseTimeLowerBound, ChaseTimeUpperBound);


        if (GameObject.Find("SurviveTimer") == null)
        {
            Debug.Log("No survival timer found! This ghost doesn't work without one!");
            return;
        }

        timeLeftCounter = GameObject.Find("SurviveTimer").GetComponent<SurvivalTimer>();




    }

    // Update is called once per frame
    void FixedUpdate()
    {
        playerPos.x = Player.transform.position.x;
        playerPos.y = Player.transform.position.y;

        ourPos.x = transform.position.x;
        ourPos.y = transform.position.y;

        us2Player = playerPos - ourPos;


        switch (state)
        {
            case BulletGhostStates.CHASE:
                //chasing behavior (default)
                //advance towards player (potentially faster if player is farther)
                //after a semi-random length of time, transition to SELECTATTACK


                targetVel = us2Player.normalized * speed;

                if (Time.time >= phaseStartTime + phaseLength)
                {
                    phaseStartTime = Time.time;
                    phaseLength = ThinkingTime;
                    state = BulletGhostStates.SELECTATTACK;
                }

                break;

            case BulletGhostStates.SELECTATTACK:
                //attack selection behavior
                //slow down to a stop for a while while choosing
                //choose dash or fire (multiple fire patterns? either way same ghost state, probably)
                //then transition to appropriate attack state immediately


                targetVel = Vector2.zero;
                if (Time.time >= phaseStartTime + phaseLength)
                {
                    phaseStartTime = Time.time;

                    if (Random.Range(0, 2) == 0) //TODO set back to (0, 2), this is for bullet testing
                    {
                        dashesLeft = numDashes;
                        phaseLength = DashPrepTime;
                        state = BulletGhostStates.PREPAREDASH;
                        dashDirection = us2Player.normalized;
                    }
                    else
                    {
                        phaseLength = fireTime;
                        state = BulletGhostStates.FIRE;
                    }
                }

                break;

            case BulletGhostStates.PREPAREDASH:
                //dash preparation behavior
                //rear back for a dash
                //then transition to DASH

                targetVel = dashDirection * DashSpeed * -1;

                if (Time.time >= phaseStartTime + phaseLength)
                {
                    targetVel = Vector2.zero;
                    phaseStartTime = Time.time;
                    phaseLength = DashTime;
                    state = BulletGhostStates.DASH;
                }

                break;

            case BulletGhostStates.DASH:
                //dashing behavior
                //dash in decided upon direction at a particular speed for a particular distance
                //then transition to COOLDOWN
                targetVel = Vector2.zero;
                velocity = Vector2.zero;
                HandleMove(dashDirection * DashSpeed, 1f);

                if (Time.time >= phaseStartTime + phaseLength)
                {
                    
                    phaseStartTime = Time.time;

                    dashesLeft--;

                    //dash as many times as specified
                    if (dashesLeft > 0)
                    {
                        velocity = dashDirection * 0.1f;
                        phaseLength = DashPrepTime;
                        dashDirection = us2Player.normalized;
                        state = BulletGhostStates.PREPAREDASH;
                    } else
                    {
                        velocity = dashDirection * speed;
                        phaseLength = CooldownTime;
                        state = BulletGhostStates.COOLDOWN;
                    }
                }


                break;

            case BulletGhostStates.FIRE:
                //firing bevahor
                //stay still and repeatedly spray bullets until done 

                if (Time.time >= timeLastBulletWasShot + bulletCooldown)
                {
                    timeLastBulletWasShot = Time.time;
                    //fire bullet
                    Vector2 bulletAngle = getRotatedVector(us2Player, Random.Range(bulletSpread * -0.5f, bulletSpread * 0.5f));
                    fireBullet(bulletAngle);
                }

                if (Time.time >= phaseStartTime + phaseLength)
                {
                    phaseStartTime = Time.time;
                    phaseLength = CooldownTime;
                    state = BulletGhostStates.COOLDOWN;
                }
                break;

            case BulletGhostStates.COOLDOWN:
                //post-attack cooldown behavior
                //either shy away from player or move towards them very slowly
                //maybe become a bit transparent in this form, maybe be unable to hurt
                //after a while, transition back to CHASE
                targetVel = Vector2.zero;

                if (Time.time >= phaseStartTime + phaseLength)
                {
                    phaseStartTime = Time.time;
                    phaseLength = Random.Range(ChaseTimeLowerBound, ChaseTimeUpperBound);
                    state = BulletGhostStates.CHASE;
                }

                break;

            default:
                Debug.Log("uh. invalid state. Whoops");
                targetVel = Vector2.zero;
                break;

        }

        //whatever targetVel is, adjust velocity towards it
        Vector2 deltaV = targetVel - velocity;
        Vector2 accel = (deltaV.magnitude >= 0.1) ? deltaV.normalized * speed : Vector2.zero;

        velocity = velocity + (accel * Time.deltaTime);


        //Whatever Velocity is, move it
        if (state != BulletGhostStates.DASH) //except if we're dashing, dashing uses separate rules
        {
            HandleMove(velocity, 1f); //we use 1 because speed is used in the making of the velocity vector.
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

    private Vector2 getRotatedVector(Vector2 input, float angleToRotate)
    {
        //first, get the angle of the original vector
        float inputAngle = (input.x >= 0) ? Vector2.Angle(Vector2.up, input) : 360f + (Vector2.Angle(Vector2.up, input) * -1);

        //then, add to or subtract from that angle (and convert to radians)
        float rotatedAngle = (inputAngle + angleToRotate) * Mathf.Deg2Rad;

        //then, make a new vector using sin and cos with that angle
        float xComp = Mathf.Sin(rotatedAngle);
        float yComp = Mathf.Cos(rotatedAngle);
        return new Vector2(xComp, yComp);

    }

    private void fireBullet(Vector2 aim)
    {
        Vector3 aim3 = Vector3.zero;
        aim3.x = aim.x;
        aim3.y = aim.y;
        GameObject newBullet = GameObject.Instantiate(BulletPrefab);
        newBullet.transform.position = transform.position + aim3;
        Bullet bulletBrain = newBullet.GetComponent<Bullet>();
        bulletBrain.setVelAndGiveTimer(aim.normalized * fireStrength, timeLeftCounter);
    }

    public void die()
    {
        //DespawnWithEffect();
        GameObject.Destroy(gameObject);
    }


    //also shamelessly stolen from MobGhost
    private IEnumerator DespawnWithEffect()
    {
        if (despawnParticles != null)
        {
            ParticleSystem particles = Instantiate(despawnParticles, transform.position, Quaternion.identity);
            float duration = particles.main.duration + particles.main.startLifetime.constantMax;
            particles.Play();
            Destroy(particles.gameObject, duration);
        }

        yield return new WaitForSeconds(0.1f);

        if (onGhostDespawned != null)
        {
            onGhostDespawned(this);
        }

        GameObject.Destroy(gameObject);
    }

}
