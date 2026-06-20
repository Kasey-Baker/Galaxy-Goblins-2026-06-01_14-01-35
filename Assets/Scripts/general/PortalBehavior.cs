using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalBehavior : MonoBehaviour
{
    [SerializeField] string levelToSendTo;
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
        }
    }
}
