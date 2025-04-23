using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{

    GameObject player;
    Vector2 playerPos;
    Vector2 targetPos;

    Vector2 camPos;

    Vector2 toTarget;

    public float XUpperBound;
    public float XLowerBound;
    public float YUpperBound;
    public float YLowerBound;

    public float speed;

    private float targetMaxX;
    private float targetMinX;
    private float targetMaxY;
    private float targetMinY;

    Vector2 VelVec;


    public Material currMat;



    void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        Camera mainCam = Camera.main;
        float camHalfHeight = mainCam.orthographicSize;
        float camHalfWidth = camHalfHeight * mainCam.aspect;
        targetMaxX = XUpperBound - camHalfWidth;
        targetMinX = XLowerBound + camHalfWidth;
        targetMaxY = YUpperBound - camHalfHeight;
        targetMinY = YLowerBound + camHalfHeight;
    }


    void Update()
    {
        playerPos.x = player.transform.position.x;
        playerPos.y = player.transform.position.y;

        camPos.x = gameObject.transform.position.x;
        camPos.y = gameObject.transform.position.y;

        //set targetPos to playerPos, but clamped to (min x coordinate considering borders and size)
        targetPos.x = Mathf.Clamp(playerPos.x, targetMinX, targetMaxX);
        targetPos.y = Mathf.Clamp(playerPos.y, targetMinY, targetMaxY);

        toTarget = targetPos - camPos;

        //move camera towards target position with a speed proportional to how far the distance is
        VelVec = toTarget * speed;

        transform.Translate(VelVec * Time.deltaTime);

    }    
    
    //Apply Visual Effects
    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (currMat != null)
        {
            Graphics.Blit(src, dest, currMat);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }
}
