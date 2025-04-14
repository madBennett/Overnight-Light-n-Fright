using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ControlManager : NetworkBehaviour
{
    //Movement
    [SerializeField] private float normSpeed = 5f;
    private float currSpeed;
    private Vector2 movement;

    [SerializeField] private GameObject PlayerObj;
    [SerializeField] private Rigidbody2D rigidBody;

    [SerializeField] private GameObject Flashlight;

    // Start is called before the first frame update
    void Start()
    {
        currSpeed = normSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        //Do nothing if this is the incorrect client object
        if (!IsOwner)
            return;

        //move the player based on user input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        //move the player according to inputs on the server
        MoveServerRpc(movement, currSpeed);

        //flash light
        Flashlight.SetActive(Input.GetMouseButton(0));
    }

    [ServerRpc]
    public void MoveServerRpc(Vector2 movement, float curSpeed)
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
