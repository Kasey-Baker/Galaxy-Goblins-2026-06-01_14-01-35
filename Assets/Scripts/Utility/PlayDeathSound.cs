using UnityEngine;

public class PlayDeathSound : MonoBehaviour
{
    [SerializeField] AudioSource mySource;
    [SerializeField] AudioClip mySound;
    [SerializeField] float myVolume;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mySource.PlayOneShot(mySound, myVolume);
        Destroy(gameObject, mySound.length);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSound(AudioClip soundToPlay, float audioVolume)
    {
        mySound = soundToPlay;
        myVolume = audioVolume;
    }
}
