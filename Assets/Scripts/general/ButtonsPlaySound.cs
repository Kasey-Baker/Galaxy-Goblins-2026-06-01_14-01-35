using UnityEngine;
using UnityEngine.Audio;

public class ButtonsPlaySound : MonoBehaviour
{

    public AudioClip buttonSound;
    private AudioSource audioSource;
    [SerializeField] GameObject mySfxPlayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        //DontDestroyOnLoad(audioSource);
        

    }

    public void PlaySound()
    {
        //audioSource.PlayOneShot(buttonSound);
        GameObject myClickSfxPlayer = Instantiate(mySfxPlayer);
        myClickSfxPlayer.GetComponent<PlayDeathSound>().SetSound(buttonSound, audioSource.volume);
        myClickSfxPlayer.GetComponent<PlayDeathSound>().SetAsPersistent();
    }
}
