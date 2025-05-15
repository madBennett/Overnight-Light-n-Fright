using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSliderControl : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private AudioManager AM;

    private void Start()
    {
        volumeSlider = GetComponent<Slider>();
        AM = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    public void UpdateVolume()
    {
        if (AM == null)
        {
            AM = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        }
        AM.volume = volumeSlider.value;
    }
}
