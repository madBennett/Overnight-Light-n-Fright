using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeManager : MonoBehaviour
{
    //Varibles to Set a random Return point
    [SerializeField] private List<Transform> ReturnSpawnLocs = new List<Transform>();
    [SerializeField] private GameObject ReturnPoint;
    
    //Varibles to Set a random a random number of Disappearingwalls
    static public float oddsOfActiveDisaperingWall = 0.5f;

    //varibles to spawn ghosts
    [SerializeField] private GameObject GhostPrefab;
    [SerializeField] private int numGhostToSpawn = 10;
    [SerializeField] private float returnSpawnRoundTime = 60f;
    private float roundStartTime = 0f;
    [SerializeField] private float maxXCord = 20f;
    [SerializeField] private float maxYCord = 12f;

    // Start is called before the first frame update
    void Start()
    {
        //set varibles for random spawn
        int randReturnIndex = Random.Range(0, ReturnSpawnLocs.Count);
        ReturnPoint.transform.position = ReturnSpawnLocs[randReturnIndex].position;

        SpawnGhosts();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - roundStartTime >= returnSpawnRoundTime)
        {
            SpawnGhosts();
        }
    }

    private void SpawnGhosts()
    {
        for (int i = 0; i< numGhostToSpawn; i++)
        {
            //find point to spawn
            Vector3 spawnPos = new Vector3(Random.Range(-1 * maxXCord, maxXCord), Random.Range(-1 * maxYCord, maxYCord), 0);


            //spawn ghosts
            Instantiate(GhostPrefab, spawnPos, Quaternion.identity);

            //reset timer
            roundStartTime = Time.time;
        }
    }
}
