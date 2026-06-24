using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadUI : MonoBehaviour
{
    [SerializeField] string mySceneName;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        mySceneName = SceneManager.GetActiveScene().name;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckScene();
    }

    void CheckScene()
    {
        if(SceneManager.GetActiveScene() != null)
        {
            if (SceneManager.GetActiveScene().name != mySceneName)
            {
                Destroy(gameObject);
            }
        }
    }
}
