using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskCompleteTrigger : MonoBehaviour
{
    public string taskID; // Like "Task1", "Task2", etc.

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerProgress.Instance.CompleteTask(taskID);
            Destroy(gameObject); // You can destroy the trigger so it can't happen twice
        }
    }
}
