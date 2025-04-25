using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    //timer
    [SerializeField] private float timeRemaining = 300;
    [SerializeField] private TMP_Text TimerText;
    public ValueBar TimeBar;

    //energy
    public static float currEnergy = 0f;
    public float maxEnergy = 100f;
    public ValueBar EnergyBar;


    // Start is called before the first frame update
    void Start()
    {
        //set Energy
        currEnergy = maxEnergy;
        EnergyBar.setMaxValue(maxEnergy);

        //get objects
        EnergyBar = GameObject.FindGameObjectWithTag("EnergyBar").GetComponent<ValueBar>();
        TimeBar = GameObject.FindGameObjectWithTag("TimeBar").GetComponent<ValueBar>();

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

        TimerText.text = "Time Remaining" + remainingMins + ":" + remaingingSecs;
    }

    public void ChangePlayerEnergy(float changeAmt)
    {
        currEnergy += changeAmt;

        //check it is not below zero or above max
        if (currEnergy > maxEnergy)
        {
            currEnergy = maxEnergy;
        }
        else if (currEnergy < 0)
        {
            currEnergy = 0;
        }

        //set value bar
        EnergyBar.setValue(currEnergy);
    }
}
