using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryPackBehavior : MonoBehaviour
{
    //set default varibles
    [SerializeField] private float batteryAmount = 10f;
    [SerializeField] private float despawnCooldDown = 2f;
    [SerializeField] private float spawnTime;

    // Start is called before the first frame update
    void Start()
    {
        //set cooldown time
        spawnTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if ((Time.time - spawnTime) >= despawnCooldDown)
        {
            //destroy on cooldown up
            RemoveObject();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //on collsion with the player heal them and despawn
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.SendMessage("UpdateEnergy", batteryAmount);
            Destroy(gameObject);
            Debug.Log("Destroy)");
        }
    }

    private void RemoveObject()
    {
        //destroy object
        Destroy(gameObject);
    }
}
