using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    }

    public void ExitGame()
    {

    }
}
