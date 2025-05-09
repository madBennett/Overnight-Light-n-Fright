using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuControler : MonoBehaviour
{
    public void StartGame()
    {
        GameState.hasPlayedMainRoomIntro = false;
        GameState.ResetGame();
        SceneManager.LoadScene("Lobby Scene");
    }

    public void ExitGame()
    {
        //exit app
        Application.Quit();
    }
}
