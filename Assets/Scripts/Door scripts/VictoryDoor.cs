using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryDoor : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Color unlockedColor = Color.green;
    public Color lockedColor = Color.gray;

    public string[] requiredTasks; // List of tasks that must be done

    private bool isUnlocked = false;

    public GameObject successUIPopup; // Drag your UI pop-up here

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = lockedColor;
    }

    private void Update()
    {
        if (!isUnlocked && AllTasksCompleted())
        {
            UnlockDoor();
        }
    }

    private bool AllTasksCompleted()
    {
        foreach (string task in requiredTasks)
        {
            if (!PlayerProgress.Instance.HasCompletedTask(task))
            {
                return false;
            }
        }
        return true;
    }

    private void UnlockDoor()
    {
        isUnlocked = true;
        spriteRenderer.color = unlockedColor;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isUnlocked)
        {
            if (successUIPopup != null)
            {
                successUIPopup.SetActive(true);
            }
        }
    }

}
