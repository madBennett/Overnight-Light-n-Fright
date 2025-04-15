using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGhostBehavior : AbstractGhostBehavior
{
    public override void StartMove()
    {
        moveStartTime = Time.time;

        //choose a direction
        movement = new Vector2(0,1);

        //choose effect
        effectToApply = (EffectTypes)(Random.Range(0, (int)EffectTypes.NUM_EFFECTS));

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

    public override void Attack(PlayerBehavior player)
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
