using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SaveSystemB : MonoBehaviour
{
    public static SaveSystemB instance;

    [Header("UI Reference")]
    [SerializeField] private GameObject savePromptMenu;

    [Header("Stored Data Vector")]
    public List<string> savedItemsVector = new List<string>();

    private LevelSystem levelSystem;
    //private PlayerInventory playerInventory; // change this to what the player inventory is

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        FetchReferences();
    }

    private void FetchReferences()
    {
        if (GameManager.instance != null && GameManager.instance.player != null)
        {
            levelSystem = GameManager.instance.player.GetComponent<LevelSystem>();
            //playerInventory = GameManager.instance.player.GetComponent<PlayerInventory>();
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

    public void CloseSavePrompt()
    {
        if (savePromptMenu != null)
        {
            savePromptMenu.SetActive(false);
            if (GameManager.instance != null) GameManager.instance.stateUnpaused();
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
        /*
        if  (playerInventory != null)
        {
            savedItemsVector.Clear();

            foreach (ItemData item in playerInventory.ownedItems)
            {
                if (item != null)
                {
                    savedItemsVector.Add(item.itemName);
                }
            }

            string serializedItems = string.Join(",", savedItemsVector);
            PlayerPrefs.SetString("SavedItems", serializedItems);
        }
        */

        PlayerPrefs.SetInt("SavedSceneIndex", SceneManager.GetActiveScene().buildIndex);
        PlayerPrefs.SetInt("HasSaveData", 1);
        PlayerPrefs.Save();

        Debug.Log("Game Saved Successfully!");
        CloseSavePrompt();
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
            //playerInventory = GameManager.instance.player.GetComponent<PlayerInventory>();

            if (levelSystem != null)
            {
                levelSystem.currentLevel = PlayerPrefs.GetInt("SavedLevel", 1);
                levelSystem.currentScore = PlayerPrefs.GetInt("SavedScore", 0);
                levelSystem.bulletDamage = PlayerPrefs.GetFloat("SavedDamage", 10f);

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

            /*
            if (playerInventory != null)
            {
                string serializedItems = PlayerPrefs.GetString("SavedItems", "");

                savedItemsVector.Clear();

                if (!string.IsNullOrEmpty(serializedItems))
                {
                    string[] loadedItemNames = serializedItems.Split(',');
                    savedItemsVector.AddRange(loadedItemNames);

                    playerInventory.ownedItems.Clear();

                    foreach (string itemName in savedItemsVector)
                    {
                        playerInventory.LoadItemByName(itemName);
                    }
                }
            }*/
            
        }

        GameManager.instance.stateUnpaused();
        Debug.Log("Game Loaded Successfully!");
    }
}
