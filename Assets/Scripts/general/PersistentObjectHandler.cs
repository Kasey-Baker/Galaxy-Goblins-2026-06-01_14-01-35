using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentObjectHandler : MonoBehaviour
{
    [SerializeField] private string targetSceneName = "Main Menu";

    private void OnEnable()
    {
        // Subscribe to the sceneLoaded event when the object becomes active
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Unsubscribe from the sceneLoaded event when the object becomes inactive
        SceneManager.sceneLoaded -= OnSceneLoaded;
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
