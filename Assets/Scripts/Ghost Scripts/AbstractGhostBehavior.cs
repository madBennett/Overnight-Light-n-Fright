using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GhostStates
{
    IDLE,
    START_MOVE,
    MOVE,
    ATTACK,
    START_RUN,
    RUN,
    NUM_STATES
}

public enum EffectTypes
{
    VISUAL_DISTORTION,
    REVERSE_CONTROLS,
    STUN,
    DAMAGE,
    TELEPORT,
    NUM_EFFECTS
}

public abstract class AbstractGhostBehavior : MonoBehaviour
{
    public GhostStates currState;
    public bool isActive = false; //bool to determine if the ghost should be moving or not

    //varibles for idle state
    public float idleEnterTime;
    public float idleTime = 1f;

    //varibles for start move state
    public Vector3 startPos;
    public Vector3 moveDestination;
    public EffectTypes effectToApply;
    public float moveStartTime;

    //Varibles for move
    public float speed = 0.01f;
    public Vector2 screenBoundry;
    public float maxMoveTime = 3f;

    //Varibles for Run state
    public Vector3 playerPos;


    // Start is called before the first frame update
    void Start()
    {
        currState = GhostStates.IDLE;
        idleEnterTime = Time.time;
        effectToApply = EffectTypes.VISUAL_DISTORTION;
        screenBoundry = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }

    private void Update()
    {
        switch(currState)
        {
            case GhostStates.IDLE:
                //if the ghost is not active do not allow out of the idle state
                //in addition if the ghost is active but enters the idle state is must stay in it for x time
                if (isActive && (Time.time - idleEnterTime >= idleTime))
                {
                    currState = GhostStates.START_MOVE;
                }
                break;
            case GhostStates.START_MOVE:
                StartMove();
                break;
            case GhostStates.MOVE:
                Move();
                break;
            case GhostStates.START_RUN:
                StartRun();
                break;
            case GhostStates.RUN:
                Run();
                break;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Flashlight")
        {
            //on collision with flashlight
            currState = GhostStates.RUN;
            playerPos = collision.gameObject.transform.position;
        }

        if (collision.gameObject.tag == "Player")
        {
            //if collision with a player attack
            Attack();
        }
    }

    public abstract void StartMove();

    public abstract void Move();

    public abstract void Attack();

    public abstract void StartRun();

    public abstract void Run();
}
