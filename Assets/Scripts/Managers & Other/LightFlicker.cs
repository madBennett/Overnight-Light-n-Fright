using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    private UnityEngine.Rendering.Universal.Light2D lightO;
    private float desync;
    public float baseStrength;
    public float timeCoefficient;
    public float alterStrength;

    // Start is called before the first frame update
    void Start()
    {
        desync = Random.Range(0f, 10f);
        GameObject childObj = transform.GetChild(0).gameObject;
        lightO = childObj.GetComponent<UnityEngine.Rendering.Universal.Light2D>();
    }

    void FixedUpdate()
    {
        int incOrDec = Random.Range(0, 2);
        if (incOrDec == 0)
        {
            desync += 0.01f;
        } else
        {
            desync -= 0.01f;
        }

            float lightVal = baseStrength + (Mathf.Sin(Time.time * timeCoefficient + desync) * alterStrength);
        lightO.intensity = lightVal;
    }
}
