using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeManager : MonoBehaviour
{
    //Varibles to Set a random Key point
    [SerializeField] private List<Transform> KeySpawnLocs = new List<Transform>();
    [SerializeField] private GameObject KeyPoint;

    //Varibles to Set a random a random number of Disappearingwalls
    public float oddsOfActiveDisaperingWall = 0.5f;
    [SerializeField] private List<GameObject> DisappearingWalls = new List<GameObject>();

    //varibles to spawn ghosts
    [SerializeField] private GameObject GhostPrefab;
    [SerializeField] private int numGhostToSpawn = 5;
    [SerializeField] private float KeySpawnRoundTime = 60f;
    private float roundStartTime = 0f;
    [SerializeField] private float maxXCord = 20f;
    [SerializeField] private float maxYCord = 12f;

    // Start is called before the first frame update
    void Start()
    {
        //set varibles for random spawn
        int randKeyIndex = Random.Range(0, KeySpawnLocs.Count);
        KeyPoint.transform.position = KeySpawnLocs[randKeyIndex].position;

        SpawnGhosts();
        ResetDisappearingWalls();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - roundStartTime >= KeySpawnRoundTime)
        {
            SpawnGhosts();

            ResetDisappearingWalls();
        }
    }

    private void SpawnGhosts()
    {
        for (int i = 0; i < numGhostToSpawn; i++)
        {
            //find point to spawn
            Vector3 spawnPos = new Vector3(Random.Range(-1 * maxXCord, maxXCord), Random.Range(-1 * maxYCord, maxYCord), 0);


            //spawn ghosts
            Instantiate(GhostPrefab, spawnPos, Quaternion.identity);

            //reset timer
            roundStartTime = Time.time;
        }
    }

    private void ResetDisappearingWalls()
    {
        for (int i = 0; i < DisappearingWalls.Count; i++)
        {
            DisappearingWalls[i].SetActive(Random.Range(0f, 1f) >= oddsOfActiveDisaperingWall);
        }
    }
}
