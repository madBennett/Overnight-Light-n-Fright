using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public static bool hasPlayedMainRoomIntro = false;
    public static bool returnedFromShootTask = false;

    public static void ResetGame()
    {
        hasPlayedMainRoomIntro = false;
        returnedFromShootTask = false;

        if (PlayerProgress.Instance != null)
        {
            PlayerProgress.Instance.ResetAllTasks();
        }
    }
}
