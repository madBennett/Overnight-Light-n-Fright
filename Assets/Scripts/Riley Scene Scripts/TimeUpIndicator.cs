using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeUpIndicator : MonoBehaviour
{
    public float upwardsSpeed = 5f;
    Vector3 moveVec;
    public float lifespan = 0.6f;
    private float life;

    private SpriteRenderer spri;

    void Start()
    {
        life = lifespan;
        spri = GetComponent<SpriteRenderer>();
        moveVec = new Vector3(0f, upwardsSpeed, 0f);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //move up by speed

        transform.Translate(moveVec * Time.deltaTime);
        life -= Time.deltaTime;

        Color currColor = spri.color;
        currColor.a = life / lifespan;
        spri.color = currColor;

        if (life <= 0f)
        {
            GameObject.Destroy(gameObject);
        }
    }
}
