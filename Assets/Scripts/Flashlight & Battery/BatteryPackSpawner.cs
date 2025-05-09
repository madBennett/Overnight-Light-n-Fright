using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryPackSpawner : MonoBehaviour
{   
    //cooldown varibles
    [SerializeField] private float batterySpawnCooldDown = 5f;
    [SerializeField] private float lastBatterySpawnTime;
    //reference to battery pack prefab
    [SerializeField] private GameObject batteryPack;
    //random location range for battery pack spawns
    [SerializeField] private Vector3 randLoc = new Vector3(0, 0, 1);
    [SerializeField] private Vector2 xRange = new Vector2(-0, 0);
    [SerializeField] private Vector2 yRange = new Vector2(-0, 0);

    //audio
    [SerializeField] private AudioClip SpawnClip;
    public AudioSource audioSource;
    public float volume = 1f;

    // Start is called before the first frame update
    private void Start()
    {
        //set time for cool down
        lastBatterySpawnTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if ((Time.time - lastBatterySpawnTime) >= batterySpawnCooldDown)
        {
            //on cooldown up choose a random location and spawn a battery pack there
            randLoc.x = Random.Range(xRange.x, xRange.y);
            randLoc.y = Random.Range(yRange.x, yRange.y);
            //spawn battery pac at the specifed location
            Instantiate(batteryPack, randLoc, Quaternion.identity);
            //play audio
            //reset cooldown
            lastBatterySpawnTime = Time.time;
        }
    }
}
