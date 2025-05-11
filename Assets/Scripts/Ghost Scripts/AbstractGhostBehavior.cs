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
    public float speed;
    public Rigidbody2D rigidBody;

    protected Animator animator;

    //audio
    protected AudioManager AM;
    [SerializeField] protected AudioSource ghostAudioSource;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        //set default values
        effectToApply = EffectTypes.VISUAL_DISTORTION;
        rigidBody = GetComponent<Rigidbody2D>();
        Player = GameObject.FindGameObjectWithTag("Player");
        EffectsManager = GameObject.FindGameObjectWithTag("EffectManager").GetComponent<EffectsManager>();
        animator = GetComponent<Animator>();
        AM = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        ghostAudioSource = GetComponent<AudioSource>();
    }

    protected virtual void HandleMove(Vector2 movement, float speed)
    {
        animator.SetBool("isWalking", true);

        if (movement != Vector2.zero)
        {
            animator.SetBool("isWalking", true);
            animator.SetFloat("InputX", movement.x);
            animator.SetFloat("InputY", movement.y);
            animator.SetFloat("LastInputX", movement.x);
            animator.SetFloat("LastInputY", movement.y);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        animator.SetFloat("InputX", movement.x);
        animator.SetFloat("InputY", movement.y);

        rigidBody.velocity = speed * movement;
    }

    protected virtual void PlayGhostAudio(AudioClipTypes audio)
    {
        AM.PlayAudio(audio, ghostAudioSource);
    }
}
