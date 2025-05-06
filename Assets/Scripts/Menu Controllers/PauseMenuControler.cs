using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuControler : MonoBehaviour
{
    [SerializeField] private GameObject PauseMenu;

    public void OpenPauseMenu()
    {
        PauseMenu.SetActive(true);
        Time.timeScale = 0;
    }
    public void ClosePauseMenu()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void ReturnToMainMenu()
    {

        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void ExitGame()
    {
        //exit app
        Application.Quit();
    }
}
