using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementStates
{
    DEFAULT,
    DASH,
    STUN,
    REVERSE
}

public class ControlManager : MonoBehaviour
{
    //Movement
    public MovementStates currMoveState;
    [SerializeField] private float normSpeed = 5f;
    public  float currSpeed;
    public Vector2 movement;

    [SerializeField] private GameObject PlayerObj;
    [SerializeField] private Rigidbody2D rigidBody;

    //for flashlight controls
    [SerializeField] private GameObject Flashlight;

    private bool lastFrameClick = false;

    // Start is called before the first frame update
    void Start()
    {
        currSpeed = normSpeed;
        Flashlight.SetActive(false);

        //get componates for movement
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        if (currMoveState != MovementStates.STUN)
        {
            //move the player based on user input
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            //move the player according to inputs on the server
            Move();

            //flash light
            setFlashlightState();
        }
    }

    public void Move()
    {
        //Reverse Movement if Applicable
        if (currMoveState == MovementStates.REVERSE)
        {
            movement *= -1;
        }
        //prevent odd input
        movement.Normalize();

        RotatePlayer();

        //move through rigid body
        rigidBody.velocity = movement * currSpeed;
    }

    public void RotatePlayer()
    {
        //rotate player accordingly
        if (movement.x != 0)
        {
            //rotate left or right
            PlayerObj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, movement.x * -90));
        }

        if (movement.y == 1)
        {
            //rotate up
            PlayerObj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        else if (movement.y == -1)
        {
            //rotate down
            PlayerObj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
        }
    }

    private void setFlashlightState()
    {
        //Change nothing if the player has no energy
        if (GameManager.currEnergy > 0)
        {
            //if click was not on last frame and is on this frame, switch the current state of flashlight.

            if (!lastFrameClick && Input.GetMouseButton(0))
            {
                Flashlight.SetActive(!Flashlight.activeSelf);
            }

            lastFrameClick = Input.GetMouseButton(0);
        }
    }
}
