using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeWallBehavior : MonoBehaviour
{
    [SerializeField] private bool isHaunted = false;
    [SerializeField] private MazeGhostBehavior Ghost = null;

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

    public void StartHaunt(MazeGhostBehavior ghost)
    {
        if (!isHaunted)
        {
            isHaunted = true;
            Ghost = ghost;
        }
    }

    private void EndHaunt(bool hitPlayer)
    {
        if (Ghost != null)
        {
            Ghost.EndHide(hitPlayer);
            Ghost = null;
            isHaunted = false;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            EndHaunt(true);
        }
        else if (collision.gameObject.tag == "Flashlight")
        {
            EndHaunt(false);
        }
    }
}
