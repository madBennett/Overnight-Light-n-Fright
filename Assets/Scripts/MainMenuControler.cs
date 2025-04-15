using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class MainMenuControler : NetworkBehaviour
{
    string mainGameSceneName = "Main Game";

    string currentSceneName;

    //test Scenes
    public GameObject TestMenu;
    string[] testSceneNames = { "Algharbi_Test Scene", "Bennett_Test Scene", "Martinez_Test Scene", "Potter_Test Scene" };

    // Start is called before the first frame update
    void Start()
    {
        currentSceneName = mainGameSceneName;
        TestMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartHost()
    {
        //Start the Player as the host      //MOSTLY FOR TESTING
        NetworkManager.Singleton.StartHost();

        //load game scene
        NetworkManager.Singleton.SceneManager.LoadScene(currentSceneName, LoadSceneMode.Single);
    }
    public void StartClient()
    {
        //Start the Player as the client      //MOSTLY FOR TESTING
        NetworkManager.Singleton.StartClient();
        
            //load game scene
            NetworkManager.Singleton.SceneManager.LoadScene(currentSceneName, LoadSceneMode.Single);
    }

    //test scene controls
    public void OpenTestMenu()
    {
        TestMenu.SetActive(true);
    }

    public void CloseTestMenu()
    {
        currentSceneName = mainGameSceneName;
        TestMenu.SetActive(false);
    }

    public void StartTestScene1()
    {
        currentSceneName = testSceneNames[0];
        StartHost();
    }
    public void StartTestScene2()
    {
        currentSceneName = testSceneNames[1];
        StartHost();
    }
    public void StartTestScene3()
    {
        currentSceneName = testSceneNames[2];
        StartHost();
    }
    public void StartTestScene4()
    {
        currentSceneName = testSceneNames[3];
        StartHost();
    }
}
