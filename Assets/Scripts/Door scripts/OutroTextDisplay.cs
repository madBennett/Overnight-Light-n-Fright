using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OutroTextDisplay : MonoBehaviour
{  
    public static OutroTextDisplay Instance;

    public TextMeshProUGUI textDisplay;
    public float initialDelay = 1f;
    public float wordDelay = 0.6f;
    public float totalDelay = 2f;

    [Header("Words to display on trigger")]
    public string[] words;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        textDisplay.gameObject.SetActive(false);
    }

    public void ShowMessage()
    {
        StartCoroutine(DisplayOutroWords());
    }

    private IEnumerator DisplayOutroWords()
    {
        textDisplay.gameObject.SetActive(true);
        yield return new WaitForSeconds(initialDelay);

        foreach (string word in words)
        {
            textDisplay.text = word;

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