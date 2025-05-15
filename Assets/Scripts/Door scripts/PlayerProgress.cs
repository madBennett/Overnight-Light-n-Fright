using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProgress : MonoBehaviour
{
    public static PlayerProgress Instance;
    private HashSet<string> completedTasks = new HashSet<string>();
    private AudioManager AM;
    public OutroTextDisplay outroTextDisplay;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CompleteTask(string taskID)
    {
        completedTasks.Add(taskID);

        // play completion audio
        AM = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        AM.PlayAudio(AudioClipTypes.COLLECT_MARKER);

        //and add to timer via GameManager
        GameManager man = GameObject.Find("GameManager").GetComponent<GameManager>();
        man.AddTime();

        // play outro complete message
        OutroTextDisplay.Instance.ShowMessage();
    }

    public bool HasCompletedTask(string taskID)
    {
        return completedTasks.Contains(taskID);
    }

    public void ResetAllTasks()
    {
        completedTasks.Clear();
    }
}
