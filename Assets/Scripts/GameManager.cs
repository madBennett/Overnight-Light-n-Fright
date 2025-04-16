using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    //timer
    [SerializeField] private float timeRemaining = 300;
    [SerializeField] private TMP_Text TimerText;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timeRemaining -= Time.deltaTime;
        //verify time does not go below zero
        if (timeRemaining < 0)
        {
            timeRemaining = 0;
            //trigger game over
        }
        else
        {
            //visually display time
            UpdateTimer();
        }
    }

    private void UpdateTimer()
    {
        float remainingMins = Mathf.Floor(timeRemaining / 60);
        float remaingingSecs = Mathf.Floor(timeRemaining % 60);

        TimerText.text = "Time Remaining\n" + remainingMins + ":" + remaingingSecs;
    }
}
