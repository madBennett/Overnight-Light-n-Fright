using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeManager : MonoBehaviour
{
    //Varibles to Set a random Return point
    [SerializeField] private List<Transform> ReturnSpawnLocs = new List<Transform>();
    [SerializeField] private GameObject ReturnPoint;
    
    //Varibles to Set a random a random number of Disappearingwalls
    [SerializeField] private List<GameObject> DisappearingWalls = new List<GameObject>();
    [SerializeField] private float oddsOfActiveDisaperingWall = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        //set varibles for random spawn
        int randReturnIndex = Random.Range(0, ReturnSpawnLocs.Count);
        ReturnPoint.transform.position = ReturnSpawnLocs[randReturnIndex].position;

        //randomly deactivate walls
        for (int i = 0; i < DisappearingWalls.Count; i++)
        {
            DisappearingWalls[i].SetActive(Random.Range(0f, 1f) >= oddsOfActiveDisaperingWall);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
