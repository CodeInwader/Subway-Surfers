using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System;

public class Settings : MonoBehaviour
{
    public Slider SliderVolume;

    public Slider SliderSfx;

    public AudioMixer AudioMixer;
   

    
    void Start()
    {
        //Setting Volume
        AudioMixer.SetFloat("Volume", PlayerPrefs.GetFloat("volume"));
       SliderVolume.SetValueWithoutNotify(PlayerPrefs.GetFloat("volume"));

        SliderSfx.SetValueWithoutNotify(PlayerPrefs.GetFloat("VolumeSFX"));
        AudioMixer.SetFloat("VolumeSFX", PlayerPrefs.GetFloat("VolumeSFX"));
    }

   

    public void SetVolume(float volume)
    {
        PlayerPrefs.SetFloat("volume", volume);
        AudioMixer.SetFloat("Volume", PlayerPrefs.GetFloat("volume"));
    }

    public void SetVolumeSFX(float volume)
    {
        PlayerPrefs.SetFloat("VolumeSFX", volume);
        AudioMixer.SetFloat("VolumeSFX", PlayerPrefs.GetFloat("VolumeSFX"));
    }
}
