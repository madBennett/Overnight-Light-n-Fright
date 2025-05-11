using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskCompleteTrigger : MonoBehaviour
{
    public string taskID;
    private AudioManager AM;

    private void Start()
    {
        AM = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerProgress.Instance.CompleteTask(taskID);
            Destroy(gameObject); // You can destroy the trigger so it can't happen twice
            AM.PlayAudio(AudioClipTypes.COLLECT_MARKER);
        }
    }
}
