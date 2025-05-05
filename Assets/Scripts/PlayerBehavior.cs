using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    private GameManager GM;

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
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //deplet energy for flashlight when active
        if (Flashlight.activeSelf)
        {
            GM.ChangePlayerEnergy(-1* energyDelpetionRate*  Time.deltaTime);
        }
    }
    

    private void UpdateEnergy(float changeAmt)
    {
        GM.ChangePlayerEnergy(changeAmt);
    }
}
