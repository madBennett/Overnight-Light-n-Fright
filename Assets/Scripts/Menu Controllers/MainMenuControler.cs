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

        // Destroy persistent UI
        var persistentUI = GameObject.Find("Player UI");
        if (persistentUI != null)
        {
            Destroy(persistentUI);
        }

        // Destroy persistent GameManager
        var gameManager = GameObject.Find("GameManager");
        if (gameManager != null)
        {
            Destroy(gameManager);
        }

        // Destroy persistent AudioManager
        var audioManager = GameObject.Find("AudioManager");
        if (audioManager != null)
        {
            Destroy(audioManager);
        }

        SceneManager.LoadScene("Lobby Scene");
    }

    public void ExitGame()
    {
        //exit app
        Application.Quit();
    }
}
