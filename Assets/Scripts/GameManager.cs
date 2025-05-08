using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    //timer
    public float timeRemaining = 300;
    public ValueBar TimeBar;

    //energy
    public static float currEnergy;
    public float maxEnergy = 100f;
    public ValueBar EnergyBar;

    // Start is called before the first frame update
    void Start()
    {
        FindObjectsInCurrScene();

        //set Energy
        currEnergy = maxEnergy;
        EnergyBar.setMaxValue(maxEnergy);

        //set Time
        TimeBar.setMaxValue(timeRemaining);
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

    public void FindObjectsInCurrScene()
    {
        //get objects
        EnergyBar = GameObject.FindGameObjectWithTag("EnergyBar").GetComponent<ValueBar>();
        TimeBar = GameObject.FindGameObjectWithTag("TimeIndicator").GetComponent<ValueBar>();
    }

    private void UpdateTimer()
    {
        TimeBar.setValue(timeRemaining);

        float remainingMins = Mathf.Floor(timeRemaining / 60);
        float remaingingSecs = Mathf.Floor(timeRemaining % 60);

        TimeBar.Text.text = "Time Remaining " + remainingMins + ":" + remaingingSecs;
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
