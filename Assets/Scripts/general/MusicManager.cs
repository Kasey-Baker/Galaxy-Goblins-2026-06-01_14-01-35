using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private string targetSceneName = "Tutorial";
    private static MusicManager instance = null;
    public static MusicManager Instance
    {
        get { return instance; }
    }
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            SceneManager.sceneLoaded -= OnSceneLoaded;
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }   
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check if the loaded scene is the target scene
        if (scene.name == targetSceneName)
        {
            // If it is, destroy this game object
            Destroy(gameObject);
        }
    }
}
