using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System;

public class Settings : MonoBehaviour
{
    public Slider _SliderVolume;

    public Slider _SliderSfx;

    public AudioMixer _AudioMixer;
   

    // Start is called before the first frame update
    void Start()
    {
        //Setting Volume
        _AudioMixer.SetFloat("Volume", PlayerPrefs.GetFloat("volume"));
       _SliderVolume.SetValueWithoutNotify(PlayerPrefs.GetFloat("volume"));

        _SliderSfx.SetValueWithoutNotify(PlayerPrefs.GetFloat("VolumeSFX"));
        _AudioMixer.SetFloat("VolumeSFX", PlayerPrefs.GetFloat("VolumeSFX"));
    }

   

    public void SetVolume(float volume)
    {
        PlayerPrefs.SetFloat("volume", volume);
        _AudioMixer.SetFloat("Volume", PlayerPrefs.GetFloat("volume"));
    }

    public void SetVolumeSFX(float volume)
    {
        PlayerPrefs.SetFloat("VolumeSFX", volume);
        _AudioMixer.SetFloat("VolumeSFX", PlayerPrefs.GetFloat("VolumeSFX"));
    }
}
