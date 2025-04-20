using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlManager : MonoBehaviour
{
    //Movement
    [SerializeField] private float normSpeed = 5f;
    public  float currSpeed;
    public Vector2 movement;

    [SerializeField] private GameObject PlayerObj;
    [SerializeField] private Rigidbody2D rigidBody;

    //for flashlight controls
    [SerializeField] private GameObject Flashlight;
    [SerializeField] private bool isFlashlightOn = false;

    // Start is called before the first frame update
    void Start()
    {
        currSpeed = normSpeed;
        Flashlight.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //move the player based on user input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        //move the player according to inputs on the server
        Move(movement, currSpeed);

        //flash light
        Flashlight.SetActive(Input.GetMouseButton(0));
    }

    public void Move(Vector2 movement, float curSpeed)
    {
        //get componates for movement
        rigidBody = GetComponent<Rigidbody2D>();
        //prevent odd input
        movement.Normalize();

        //move through rigid body
        rigidBody.velocity = movement * currSpeed;

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
}
