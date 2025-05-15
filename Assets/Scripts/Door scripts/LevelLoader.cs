using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance;

    public Animator transition;
    public float transitionTime = 1f;

    [SerializeField] private GameObject PlayerUI;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes

            PlayerUI = GameObject.FindGameObjectWithTag("PlayerUI");
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates
        }
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadLevel(sceneName));
    }

    private IEnumerator LoadLevel(string sceneName)
    {
        transition.SetTrigger("Start"); // Start fade to black
        yield return new WaitForSeconds(transitionTime);
        
        SceneManager.LoadScene(sceneName);

        // Wait one frame to let the new scene load
        yield return null;

        // Fade back in
        transition.SetTrigger("End"); // Trigger your fade-in animation

        if (sceneName == "Lobby Scene")
        {
            //refetch ui varible
            PlayerUI = GameObject.FindGameObjectWithTag("PlayerUI");
        }
        PlayerUI.SetActive(sceneName != "MainMenu" || sceneName != "GameWin" || sceneName != "GameLoss");

    }
}
