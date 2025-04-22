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

    //
    public EffectTypes currEffect = EffectTypes.DEFAULT;

    // Start is called before the first frame update
    void Start()
    {
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

    public void DamagePlayer(int damageAmt)
    {
        currHealth -= damageAmt;
        HealthBar.setValue(currHealth);
    }
}
