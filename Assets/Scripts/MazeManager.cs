using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeManager : MonoBehaviour
{
    //Varibles to Set a random Return point
    [SerializeField] private List<Transform> ReturnSpawnLocs = new List<Transform>();
    [SerializeField] private GameObject ReturnPoint;

    // Start is called before the first frame update
    void Start()
    {
        int randReturnIndex = Random.Range(0, ReturnSpawnLocs.Count);
        ReturnPoint.transform.position = ReturnSpawnLocs[randReturnIndex].position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
