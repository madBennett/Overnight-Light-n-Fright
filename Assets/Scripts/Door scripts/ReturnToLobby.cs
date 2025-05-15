using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ReturnToLobby : MonoBehaviour
{
    public DoorData doorData;
    private EffectsManager effectsManager;

    private AudioManager AM;

    public bool isLocked = true;

    private void Start()
    {
        effectsManager = FindObjectOfType<EffectsManager>();
        AM = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isLocked)
            {
                LoadLobby();
            }
            else
            {
                //play locked door audio
            }
        }
    }

    private void LoadLobby()
    {
        AM.PlayAudio(AudioClipTypes.ENTER_GATE);

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
