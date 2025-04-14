using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGhostBehavior : AbstractGhostBehavior
{
    float moveDistnce = 1f;

    public override void StartMove()
    {
        moveStartTime = Time.time;

        //choose a direction
        moveDestination = transform.position + new Vector3(0, moveDistnce, 0);

        //choose effect
        effectToApply = (EffectTypes)(Random.Range(0, (int)EffectTypes.NUM_EFFECTS));

        //change State to move
        currState = GhostStates.MOVE;
    }

    public override void Move()
    {
        //check if has reached the position
        if ((transform.position == moveDestination) || (Time.time - moveStartTime >= maxMoveTime))
        {
            currState = GhostStates.IDLE;
        }
        else
        {
            //Move in choosen direction until the desitnation is reached
            Vector2 newPos = Vector2.Lerp(transform.position, moveDestination, speed);

            //move the player
            transform.position = newPos;
        }

    }

    public override void Attack()
    {
        //
    }

    public override void StartRun()
    {
        //select movment destination and move towards it
    }

    public override void Run()
    {
        //If the ghost is in contact with the flashlight move in the oppistae dircetion of the player
    }
}
