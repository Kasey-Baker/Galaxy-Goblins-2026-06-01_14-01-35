using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalBehavior : MonoBehaviour
{
    [SerializeField] string levelToSendTo;
    [SerializeField] AudioClip myPortalTravelSound;
    [SerializeField] GameObject myDeathSoundPlayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == GameManager.instance.player)
        {
            switch (levelToSendTo)
            {
                case "Grass Level":

                    GameManager.instance.grassLevelAttempted = true;

                    break;

                case "Water Level":

                    GameManager.instance.waterLevelAttempted = true;

                    break;

                case "Volcano Level":

                    GameManager.instance.volcanoLevelAttempted = true;

                    break;

                default:

                    break;
            }


            SceneManager.LoadScene(levelToSendTo);
            GameManager.instance.difficultyLevel += 1;
            GameObject mySoundPlayer = Instantiate(myDeathSoundPlayer);
            mySoundPlayer.GetComponent<PlayDeathSound>().SetSound(myPortalTravelSound, 0.5f);
            mySoundPlayer.GetComponent<PlayDeathSound>().SetAsPersistent();
        }
    }
}
