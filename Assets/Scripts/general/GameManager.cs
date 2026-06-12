using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    [Header("Persistent Objects")]
    public GameObject[] persistentObjects;

    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    public Image playerHPBar;
    public float Points;
    public TMP_Text pointsText;

    public GameObject checkPointPopup;
    public TMP_Text gameGoalCountText;
    public TMP_Text enemyCountText;

    public bool isPaused;
    public GameObject player;
    public PlayerControls playercontrols;
    // public GameObject playerStartPos;
    public PlayerData playerStats = new PlayerData();

    int gameGoalCount;
    int enemyCount;
    //int keyGoalCount;

    float timeScaleOrig;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    

    void Awake()
    {
        if (instance != null)
        {
            CleanUpAndDestroy();
            return;
        }
        instance = this;
        timeScaleOrig = Time.timeScale;
        player = GameObject.FindWithTag("Player");
        playercontrols = player.GetComponent<PlayerControls>();
        // playerStartPos = GameObject.FindWithTag("playerStartPos");
        DontDestroyOnLoad(gameObject);
        MarkPersistentObjects();
        
            
    }

    private void MarkPersistentObjects()
    {
        foreach (GameObject obj in persistentObjects)
        {
           if (obj != null)
            {
                DontDestroyOnLoad(obj);
            }
        }
    }

    private void CleanUpAndDestroy()
    {
        foreach (GameObject obj in persistentObjects)
        {
            if (obj != null)
            {
                Destroy(obj);
            }
        }
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        pointsText.text = Points.ToString("F0");
        if (Input.GetButtonDown("Cancel"))
        {
            if (menuActive == null)
            {
                statePause();
                menuActive = menuPause;
                menuActive.SetActive(true);
            }
            else if (menuActive == menuPause)
            {
                stateUnpaused();
            }
        }
    }

    public void statePause()
    {
        isPaused = true;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void stateUnpaused()
    {
        isPaused = false;
        Time.timeScale = timeScaleOrig;
        menuActive.SetActive(false);
        menuActive = null;
    }

    public void updateEnemyCount(int amount)
    {
        enemyCount += amount;
        //enemyCountText.text = enemyCount.ToString("F0");
        //if (enemyCount <= 0)
        //{
            // start next wave
        //}

    }
    /*
    public void updateGameGoal(int amount)
    {
        gameGoalCount += amount;
        gameGoalCountText.text = gameGoalCount.ToString("F0");
        if (gameGoalCount <= 0)
        {
            // you win!
            statePause();
            menuActive = menuWin;
            menuActive.SetActive(true);
        }

    }
    */
    public int getEnemyCount()
    {
        return enemyCount;
    }
    public void YouLose()
    {
        statePause();
        menuActive = menuLose;
        menuActive.SetActive(true);
    }

    public void AddPoints(float amount)
    {
        Points += amount;
        GameManager.instance.player.GetComponent<LevelSystem>().CheckForLevelUp();
    }

    private string SavePath => Path.Combine(Application.persistentDataPath, "savegame.json");

    public void SaveGame()
    {
        // 1. Convert the data object to a JSON string representation
        string json = JsonUtility.ToJson(playerStats, true);

        // 2. Write that string onto the user's hard drive 
        File.WriteAllText(SavePath, json);
        Debug.Log($"Game Saved to: {SavePath}");
    }

    public void LoadGame()
    {
        // Check if a save file actually exists before trying to read it
        if (File.Exists(SavePath))
        {
            // 1. Read the raw text from the file
            string json = File.ReadAllText(SavePath);

            // 2. Overwrite the runtime object with the saved values
            JsonUtility.FromJsonOverwrite(json, playerStats);
            Debug.Log("Game Loaded Successfully!");
        }
        else
        {
            Debug.LogWarning("No save file found. Starting fresh!");
        }
    }


}
