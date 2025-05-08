using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    private GameManager GM;
    private AudioManager AM;

    //energy
    [SerializeField] private GameObject Flashlight;
    public float energyDelpetionRate = 1;

    // camera fields
    [Header("Camera Settings")]
    [SerializeField] private Camera playerCamera; // Assign in prefab or spawn via script
    [SerializeField] private Vector3 cameraOffset = new Vector3(0, 0, -10); // Typical 2D offset

    // Start is called before the first frame update
    void Start()
    {
        //get objects
        Flashlight = GameObject.FindGameObjectWithTag("Flashlight");
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        AM = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //deplet energy for flashlight when active
        if (Flashlight.activeSelf)
        {
            UpdateEnergy(-1* energyDelpetionRate*  Time.deltaTime);
        }
    }
    
    private void UpdateEnergy(float changeAmt)
    {
        GM.ChangePlayerEnergy(changeAmt);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            //play audio
            AM.PlayAudio(AudioClipTypes.HIT_WALL);
        }
    }
}
