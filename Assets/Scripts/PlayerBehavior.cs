using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class PlayerBehavior : NetworkBehaviour
{
    //Static Varibles
    public static int numPlayers = 0; //total number of alive players 

    //Player Identification
    public ulong PlayerId = 0;
    [SerializeField] private TMP_Text healthBarText; //text box to disaplayer player id

    //health
    [SerializeField] private NetworkVariable<float> curEnergy = new NetworkVariable<float>(0);
    public float maxEnergy = 100f;
    public ValueBar energyBar;

    // camera fields
    [Header("Camera Settings")]
    [SerializeField] private Camera playerCamera; // Assign in prefab or spawn via script
    [SerializeField] private Vector3 cameraOffset = new Vector3(0, 0, -10); // Typical 2D offset

    // Start is called before the first frame update
    void Start()
    {
        //Prevent despawn on host being transfered into a different scene if they win
        DontDestroyOnLoad(this.gameObject);

        //itteration of player count
        numPlayers += 1;

        //set player identifcation for ease of play
        PlayerId = OwnerClientId;
        healthBarText.text = "Player: " + (PlayerId + 1);

        //set health
        curEnergy.Value = maxEnergy;
        energyBar.setMaxValue(maxEnergy);
        curEnergy.OnValueChanged += EnergyChanged;//subscribe to value change on network varible
    }
        
    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            // Enable only local camera
            if (playerCamera != null)
            {
                playerCamera.enabled = true;
            }
            else
            {
                Debug.LogWarning("Player camera not assigned!");
            }
        }
        else
        {
            // Disable cameras that are not owned by the local client
            if (playerCamera != null)
            {
                playerCamera.enabled = false;
            }
        }
    }

    private void EnergyChanged(float previousValue, float newValue)
    {
        //visually display health
        energyBar.setValue(newValue);

        //trigger death if players health is too low
        if (newValue < 0)
        {
            newValue = 0;
        }

        if (newValue > maxEnergy)
        {
            newValue = maxEnergy;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Do nothing if this is the incorrect client object
        if (!IsOwner)
            return;
    }
}
