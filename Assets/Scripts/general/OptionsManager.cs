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

    private const string SAVED_MASTER_VOL = "savedMasterVol";
    private const string SAVED_MUSIC_VOL = "savedMusicVol";
    private const string SAVED_SFX_VOL = "savedSFXVol";
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
        float savedMasterVol = PlayerPrefs.GetFloat(SAVED_MASTER_VOL, 100);
        float savedMusicVol = PlayerPrefs.GetFloat(SAVED_MUSIC_VOL, 100);
        float savedSFXVol = PlayerPrefs.GetFloat(SAVED_SFX_VOL, 100);

        MasterVol.value = savedMasterVol;
        MusicVol.value = savedMusicVol;
        SFXVol.value = savedSFXVol;

            ChangeMasterVolume();
            ChangeMusicVolume();
            ChangeSFXVolume();
    }

    public void SetMasterVolumeFromSlider()
    {
        ChangeMasterVolume();
        
        PlayerPrefs.SetFloat(SAVED_MASTER_VOL, (float)MasterVol.value);
        
        PlayerPrefs.Save();
    }
    
    public void SetMusicVolumeFromSlider()
    {
        ChangeMusicVolume();
        
        PlayerPrefs.SetFloat(SAVED_MUSIC_VOL, (float)MusicVol.value);
        
        PlayerPrefs.Save();
    }
    public void SetSFXolumeFromSlider()
    {
        ChangeSFXVolume();
        
        PlayerPrefs.SetFloat(SAVED_SFX_VOL, (float)SFXVol.value);

        PlayerPrefs.Save();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
