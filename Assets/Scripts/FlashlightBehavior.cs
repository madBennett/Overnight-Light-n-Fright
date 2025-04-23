using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightBehavior : MonoBehaviour
{
    GameObject rotationPoint;
    Vector2 rotationPointPos;
    //Rigidbody2D RPBody;

    Vector2 mousePosInWorld;
    Vector3 mousePos3D;
    Vector2 mousePosOnScreen;

    Vector2 us2Mouse;
    Vector3 us2Mouse3D;

    Quaternion currentAngle;

    Camera mainCam;


    


    void Start()
    {
        rotationPoint = gameObject;
        //RPBody = rotationPoint.GetComponent<Rigidbody2D>();
        mainCam = Camera.main;
        us2Mouse3D = Vector3.zero;
    }

    void Update()
    {
        //find the angle from rotationPoint to the mouse
        //step 1: acquire vector2 positions of mouse and rotation point

        mousePosOnScreen.x = Input.mousePosition.x;
        mousePosOnScreen.y = Input.mousePosition.y;

        mousePos3D = mainCam.ScreenToWorldPoint(mousePosOnScreen);

        mousePosInWorld.x = mousePos3D.x;
        mousePosInWorld.y = mousePos3D.y;

        rotationPointPos.x = rotationPoint.transform.position.x;
        rotationPointPos.y = rotationPoint.transform.position.y;

        //step 2: acquire the vector between those positions
        us2Mouse = mousePosInWorld - rotationPointPos;

        //step 3: get the angle, between 0 and 360 degrees, from 'up' to that point
        float mouseAngle = Vector2.Angle(Vector2.up, us2Mouse);

        if (mousePosInWorld.x > rotationPointPos.x)
        {
            mouseAngle = 360 - mouseAngle;
        }

        us2Mouse3D.z = us2Mouse.x;
        us2Mouse3D.z = us2Mouse.y;

        currentAngle = Quaternion.AngleAxis(mouseAngle, Vector3.forward);

        //step 4: rotate the rotationPoint to that angle
        transform.rotation = currentAngle;

    }
}
