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

        // Use LevelLoader to handle the scene transition with fade
        if (LevelLoader.Instance != null)
        {
            LevelLoader.Instance.LoadScene("Lobby Scene");
        }
        else
        {
            // Fallback in case LevelLoader doesn't exist (optional)
            SceneManager.LoadScene("Lobby Scene");
        }    
    }

    public void OpenInstruction()
    {
        SceneManager.LoadScene("Instruction Scene");
    }

    public void ExitGame()
    {
        //exit app
        Application.Quit();
    }
}
