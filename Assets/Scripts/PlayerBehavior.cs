using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    //health
    [SerializeField] private float curEnergy = 0f;
    public float maxEnergy = 100f;

    // camera fields
    [Header("Camera Settings")]
    [SerializeField] private Camera playerCamera; // Assign in prefab or spawn via script
    [SerializeField] private Vector3 cameraOffset = new Vector3(0, 0, -10); // Typical 2D offset

    // Start is called before the first frame update
    void Start()
    {
        //set health
        curEnergy = maxEnergy;
    }

    // Update is called once per frame
    void Update()
    {
        

    }
}
