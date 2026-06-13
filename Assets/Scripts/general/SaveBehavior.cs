using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem instance;

    [Header("UI Reference")]
    [SerializeField] private GameObject savePromptMenu;

    private LevelSystem levelSystem;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        GameObject.DontDestroyOnLoad(gameObject);
        if (GameManager.instance != null && GameManager.instance.player != null)
        {
            levelSystem = GameManager.instance.player.GetComponent<LevelSystem>();
        }
    }

    public void ShowSavePrompt()
    {
        if (savePromptMenu != null)
        {
            savePromptMenu.SetActive(true);
            if (GameManager.instance != null) GameManager.instance.statePause();
        }
    }

    public void SaveGame()
    {
        if (GameManager.instance == null) return;

        PlayerPrefs.SetFloat("SavedPoints", GameManager.instance.Points);

        if (levelSystem != null)
        {
            PlayerPrefs.SetInt("SavedLevel", levelSystem.currentLevel);
            PlayerPrefs.SetInt("SavedScore", levelSystem.currentScore);
            PlayerPrefs.SetFloat("SavedDamage", levelSystem.bulletDamage);
        }

        if (GameManager.instance.playercontrols != null)
        {
            System.Reflection.FieldInfo healthField = typeof(PlayerControls).GetField("healthCurr", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (healthField != null)
            {
                float currentHP = (float)healthField.GetValue(GameManager.instance.playercontrols);
                PlayerPrefs.SetFloat("SavedHP", currentHP);
            }
        }

        PlayerPrefs.SetInt("SavedSceneIndex", SceneManager.GetActiveScene().buildIndex);
        PlayerPrefs.SetInt("HasSaveData", 1);
        PlayerPrefs.Save();

        Debug.Log("Game Saved Successfully!");
        CloseSavePrompt();
    }

    public void CloseSavePrompt()
    {
        if (savePromptMenu != null)
        {
            savePromptMenu.SetActive(false);
            if (GameManager.instance != null) GameManager.instance.stateUnpaused();
        }
    }

    public void LoadGame()
    {
        if (PlayerPrefs.GetInt("HasSaveData", 0) == 0)
        {
            Debug.LogWarning("No save data found!");
            return;
        }

        int savedScene = PlayerPrefs.GetInt("SavedSceneIndex", 1);
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(savedScene);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        if (GameManager.instance == null) return;

        GameManager.instance.Points = PlayerPrefs.GetFloat("SavedPoints", 0);

        if (GameManager.instance.player != null)
        {
            levelSystem = GameManager.instance.player.GetComponent<LevelSystem>();
            if (levelSystem != null)
            {
                levelSystem.currentLevel = PlayerPrefs.GetInt("SavedLevel", 1);
                levelSystem.currentScore = PlayerPrefs.GetInt("SavedScore", 0);
                levelSystem.bulletDamage = PlayerPrefs.GetFloat("SavedDamage", 10);

                levelSystem.Invoke("UpdateShotPattern", 0.1f);
            }

            if (GameManager.instance.playercontrols != null)
            {
                System.Reflection.FieldInfo healthField = typeof(PlayerControls).GetField("healthCurr", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (healthField != null)
                {
                    float savedHP = PlayerPrefs.GetFloat("SavedHP", 10f);
                    healthField.SetValue(GameManager.instance.playercontrols, savedHP);
                }
            }
        }

        GameManager.instance.stateUnpaused();
        Debug.Log("Game Loaded Successfully!");
    }
}