using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ValueBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public TMP_Text Text;

    public void setValue(float val)
    {
        //change slider value and graident
        slider.value = val;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    public void setMaxValue(float val)
    {
        //set up silder
        slider.maxValue = val;
        slider.value = val;

        fill.color = gradient.Evaluate(1f);
    }
}
