using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProgress : MonoBehaviour
{
    public static PlayerProgress Instance;

    private HashSet<string> completedTasks = new HashSet<string>();

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
