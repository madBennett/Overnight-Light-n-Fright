using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class IntroTextDisplay : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    public float initialDelay = 1f;
    public float wordDelay = 0.6f;
    public float totalDelay = 2f;

    [Header("Words to display")]
    public string[] words;

    private void Start()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        // Only play main room intro once
        if (currentScene == "Lobby Scene" && GameState.hasPlayedMainRoomIntro)
        {
            textDisplay.gameObject.SetActive(false);
            //GameState.returnedFromShootTask = false; // reset for future entries
            return;
        }

        StartCoroutine(DisplayIntroWords(currentScene));
    }

    private IEnumerator DisplayIntroWords(string sceneName)
    {
        if (sceneName == "Lobby Scene")
        {
            GameState.hasPlayedMainRoomIntro = true;
        }

        textDisplay.gameObject.SetActive(true);

        yield return new WaitForSeconds(initialDelay);
        
        for (int i = 0; i < words.Length; i++)
        {
            textDisplay.text = words[i];

            // Play boom sound
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayAudio(AudioClipTypes.TEXT_BOOM);
            }
            
            yield return new WaitForSeconds(wordDelay);
        }

        yield return new WaitForSeconds(totalDelay);
        textDisplay.gameObject.SetActive(false);
    }
}
