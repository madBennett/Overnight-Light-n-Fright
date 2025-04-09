using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;

public class GameManager : NetworkBehaviour
{
    //timer
    [SerializeField] private NetworkVariable<float> timeRemaining = new NetworkVariable<float>(300);
    [SerializeField] private TMP_Text TimerText;


    // Start is called before the first frame update
    void Start()
    {
        timeRemaining.OnValueChanged += TimeRemainingChanged;//subscribe to value change on network varible
    }

    // Update is called once per frame
    void Update()
    {
        timeRemaining.Value -= Time.deltaTime;
        //verify time does not go below zero
        if (timeRemaining.Value < 0)
        {
            timeRemaining.Value = 0;
            //trigger game over
        }
    }

    private void TimeRemainingChanged(float previousValue, float newValue)
    {
        //verify time does not go below zero
        if (newValue < 0)
        {
            newValue = 0;
            //trigger game over
        }

        //visually display time
        UpdateTimerCLientRPC();

    }

    [ClientRpc]
    private void UpdateTimerCLientRPC()
    {
        float remainingMins = Mathf.Floor(timeRemaining.Value / 60);
        float remaingingSecs = Mathf.Floor(timeRemaining.Value % 60);

        TimerText.text = "Time Remaining\n" + remainingMins + ":" + remaingingSecs;
    }
}
