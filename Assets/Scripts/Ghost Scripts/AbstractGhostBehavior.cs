using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class AbstractGhostBehavior : MonoBehaviour
{
    public bool isActive = false; //bool to determine if the ghost should be moving or not
    public GameObject Player;
    public EffectsManager EffectsManager;


    //varibles for start move state
    public Vector2 movement;
    public EffectTypes effectToApply;

    //Varibles for move
    public float speed = 1f;
    public Rigidbody2D rigidBody;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        //set default values
        effectToApply = EffectTypes.VISUAL_DISTORTION;
        rigidBody = GetComponent<Rigidbody2D>();
        Player = GameObject.FindGameObjectWithTag("Player");
        EffectsManager = GameObject.FindGameObjectWithTag("EffectManager").GetComponent<EffectsManager>();
    }
}
