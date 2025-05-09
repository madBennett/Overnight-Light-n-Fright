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
            if (effectsManager != null)
            {
                // Reset visual distortion shader
                effectsManager.ReturnToDefalut(EffectTypes.VISUAL_DISTORTION);
            }

            SceneManager.LoadScene("Lobby Scene");
        }
    }
}
