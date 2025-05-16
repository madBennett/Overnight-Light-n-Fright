using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoorBehavior : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite unlockedSprite;
    [SerializeField] private Sprite lockedSprite;

    [SerializeField] public string TaskID;
    [SerializeField] private ReturnToLobby returnPt;

    // Start is called before the first frame update
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        returnPt = GetComponent<ReturnToLobby>();
        spriteRenderer.sprite = returnPt.isLocked ? lockedSprite : unlockedSprite;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (PlayerProgress.Instance.HasCompletedTask(TaskID))
            {
                returnPt.LoadLobby();
            }
        }
    }

    public void UnlockDoor()
    {
        returnPt.isLocked = false;
        spriteRenderer.sprite = unlockedSprite;
    }
}
