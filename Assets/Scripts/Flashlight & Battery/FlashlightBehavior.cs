using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightBehavior : MonoBehaviour
{
    GameObject rotationPoint;
    Vector2 rotationPointPos;

    Vector2 mousePosInWorld;
    Vector3 mousePos3D;
    Vector2 mousePosOnScreen;

    Vector2 us2Mouse;
    Vector3 us2Mouse3D;

    Quaternion currentAngle;

    Camera mainCam;

    public Transform playerTransform; // reference to the player to get the facing direction

    void Start()
    {
        rotationPoint = gameObject;
        mainCam = Camera.main;
        us2Mouse3D = Vector3.zero;
    }

    void Update()
    {
        //find the angle from rotationPoint to the mouse
        //step 1: acquire vector2 positions of mouse and rotation point

        // world mouse position
        mousePosOnScreen = Input.mousePosition;
        mousePos3D = mainCam.ScreenToWorldPoint(mousePosOnScreen);
        mousePosInWorld = new Vector2(mousePos3D.x, mousePos3D.y);

        rotationPointPos = rotationPoint.transform.position;

        //step 2: acquire the vector between those positions
        us2Mouse = mousePosInWorld - rotationPointPos;

        //step 3: get the angle, between 0 and 360 degrees, from 'up' to that point
        float targetAngle = Mathf.Atan2(us2Mouse.y, us2Mouse.x) * Mathf.Rad2Deg - 90f;

        // step 4: get player's facing direction
        Vector2 playerForward = playerTransform.up;
        float playerFacingAngle = Mathf.Atan2(playerForward.y, playerForward.x) * Mathf.Rad2Deg - 90f;

        // step 5: calculate relative angle and clamp to ±90
        float angleOffset = Mathf.DeltaAngle(playerFacingAngle, targetAngle);
        angleOffset = Mathf.Clamp(angleOffset, -360f, 360f); // full rotation for now

        float clampedAngle = playerFacingAngle + angleOffset;

        // step 6: apply rotation
        transform.rotation = Quaternion.Euler(0, 0, clampedAngle);
    }
}
