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
    protected override void Start()
    {
        base.Start();
    }
    
    public void Update()
    {
        switch (currState)
        {
            case GhostStates.IDLE:
                Idle();
                break;
            case GhostStates.START_MOVE:
                StartMove();
                break;
            case GhostStates.MOVE:
                Move();
                break;
            case GhostStates.RUN:
                Run();
                break;
        }
    }

    public void StartMove()
    {
        moveStartTime = Time.time;

        //choose a direction
        movement = new Vector2(0,1);

        //choose effect
        effectToApply = (EffectTypes)(Random.Range(1, (int)EffectTypes.NUM_EFFECTS));

        //change State to move
        currState = GhostStates.MOVE;
    }

    public override void Move()
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

    public override void Idle()
    {
        //if the ghost is not active do not allow out of the idle state
        //in addition if the ghost is active but enters the idle state is must stay in it for x time
        if (isActive && (Time.time - idleEnterTime >= idleTime))
        {
            currState = GhostStates.START_MOVE;
        }
    }

    public override void Attack(PlayerBehavior player)
    {
        //
        EffectsManager.ApplyEffect(effectToApply);
    }

    public override void OnInteractWithFlashLight()
    {
        currState = GhostStates.RUN;
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
