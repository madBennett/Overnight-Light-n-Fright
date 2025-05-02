using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeWallBehavior : MonoBehaviour
{
    [SerializeField] private bool isHaunted = false;
    [SerializeField] private bool isDisappearingWall = false;

    private void Start()
    {
        HideWall();
    }

    public void HideWall()
    {
        if (isDisappearingWall)
        {
            gameObject.SetActive(Random.Range(0f, 1f) >= MazeManager.oddsOfActiveDisaperingWall);
        }
    }

    public void StartHaunt()
    {

    }
}
