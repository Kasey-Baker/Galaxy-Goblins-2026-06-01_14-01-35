using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public void SaveGameSettings(float masterVolume, float musicVolume, float sfxVolume)
    {
        PlayerPrefs.SetFloat("MasterVol", masterVolume);
        PlayerPrefs.SetFloat("MusicVol", musicVolume);
        PlayerPrefs.SetFloat("SFXVol", sfxVolume);
        PlayerPrefs.Save();
    }
}
