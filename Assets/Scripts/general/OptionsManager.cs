using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class OptionsManager : MonoBehaviour
{
    public Slider MasterVol, MusicVol, SFXVol;
    public AudioMixer audioMixer;

    public void ChangeMasterVolume()
    {
       audioMixer.SetFloat("MasterVol", Mathf.Log10(MasterVol.value) * 20);
    }
    public void ChangeMusicVolume()
    {
       audioMixer.SetFloat("MusicVol", Mathf.Log10(MusicVol.value) * 20);
    }
    public void ChangeSFXVolume()
    {
       audioMixer.SetFloat("SFXVol", Mathf.Log10(SFXVol.value) * 20);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
