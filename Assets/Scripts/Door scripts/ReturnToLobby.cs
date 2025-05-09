using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ReturnToLobby : MonoBehaviour
{
    public DoorData doorData;
    private EffectsManager effectsManager;

    private void Start()
    {
        effectsManager = FindObjectOfType<EffectsManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (LevelLoader.Instance != null)
            {
                LevelLoader.Instance.LoadScene("Lobby Scene");
            }
            else
            {
                SceneManager.LoadScene("Lobby Scene"); // fallback
            }
        }
    }
}
