using UnityEngine;
using UnityEngine.Audio;

public class ButtonsPlaySound : MonoBehaviour
{

    public AudioClip buttonSound;
    private AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound()
    {
        audioSource.PlayOneShot(buttonSound);
    }
}
