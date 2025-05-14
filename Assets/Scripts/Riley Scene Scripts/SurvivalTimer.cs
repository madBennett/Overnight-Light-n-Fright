using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; //implements the TMPro class

public class SurvivalTimer : MonoBehaviour
{
    [Header("Dynamic")] //dynamic, meaning things like The Score can change during the game, as opposed to Inscribed fields which don't change during the game

    public float startingTime = 20f;
    private float time;

    private TextMeshProUGUI uiText;
    private bool workComplete = false;

    // Start is called before the first frame update
    void Start()
    {
        uiText = GetComponent<TextMeshProUGUI>();
        time = startingTime;
    }


    void FixedUpdate()
    {
        if (time > 0 && !workComplete)
        {
            time -= Time.deltaTime;

            string timeAsString = ((int)time).ToString("#,0");
            uiText.text = timeAsString;

        } else if (!workComplete)
        {
            GameObject ghost = GameObject.Find("Bullet Ghost");
            BulletGhost ghostMind = ghost.GetComponent<BulletGhost>();
            ghostMind.die();
            workComplete = true;
            completeTask();
        }
    }

    void completeTask()
    {
        uiText.text = "Survived!";
        PlayerProgress.Instance.CompleteTask("DexTask");
    }

    public void addTime(float secs)
    {
        if (!workComplete)
        {
            time += secs;
            if (time > startingTime) time = startingTime;
        }
    }
}
