using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    //health
    [SerializeField] private float currEnergy = 0f;
    public float maxEnergy = 100f;
    [SerializeField] private float currHealth = 0f;
    public float maxHealth = 100f;

    [SerializeField] private ValueBar EnergyBar;
    [SerializeField] private ValueBar HealthBar;

    // camera fields
    [Header("Camera Settings")]
    [SerializeField] private Camera playerCamera; // Assign in prefab or spawn via script
    [SerializeField] private Vector3 cameraOffset = new Vector3(0, 0, -10); // Typical 2D offset

    // Start is called before the first frame update
    void Start()
    {
        //get objects
        EnergyBar = GameObject.FindGameObjectWithTag("Energy Bar").GetComponent<ValueBar>();
        HealthBar = GameObject.FindGameObjectWithTag("Health Bar").GetComponent<ValueBar>();

        //set health
        currEnergy = maxEnergy;
        currHealth = maxHealth;

        EnergyBar.setMaxValue(maxEnergy);
        HealthBar.setMaxValue(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    public void ChangePlayerHealth(int changeAmt)
    {
        currHealth += changeAmt;

        //check it is not below zero or above max
        if (currHealth > maxHealth)
        {
            currHealth = maxHealth;
        }
        else if (currHealth < 0)
        {
            currHealth = 0;
        }

        HealthBar.setValue(currHealth);
    }

    public void ChangePlayerEnergy(int changeAmt)
    {
        currEnergy += changeAmt;

        //check it is not below zero or above max
        if (currEnergy > maxEnergy)
        {
            currEnergy = maxEnergy;
        }
        else if (currEnergy < 0)
        {
            currEnergy = 0;
        }

        //set value bar
        EnergyBar.setValue(currEnergy);
    }
}
