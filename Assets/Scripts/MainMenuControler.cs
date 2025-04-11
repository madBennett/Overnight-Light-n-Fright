using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class MainMenuControler : NetworkBehaviour
{
    string mainGameSceneName = "Main Game";

    // Start is called before the first frame update
    void Start()
    {
        
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
        NetworkManager.Singleton.SceneManager.LoadScene(mainGameSceneName, LoadSceneMode.Single);
    }
    public void StartClient()
    {
        //Start the Player as the client      //MOSTLY FOR TESTING
        NetworkManager.Singleton.StartClient();
        
            //load game scene
            NetworkManager.Singleton.SceneManager.LoadScene(mainGameSceneName, LoadSceneMode.Single);
    }
}
