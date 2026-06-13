using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using UnityEngine.EventSystems;

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
    public 

    int gameGoalCount;
    int enemyCount;
    //int keyGoalCount;

    float timeScaleOrig;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] GameObject pauseButtonSelect;
    [SerializeField] GameObject winButtonSelect;
    [SerializeField] GameObject loseButtonSelect;
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
        if (player != null)
        {
            playercontrols = player.GetComponent<PlayerControls>();
        }
        // playerStartPos = GameObject.FindWithTag("playerStartPos");
        DontDestroyOnLoad(gameObject);
        MarkPersistentObjects();
        menuActive = null;
            
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
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(pauseButtonSelect);
    }

    public void stateUnpaused()
    {
        isPaused = false;
        Time.timeScale = timeScaleOrig;
        if (menuActive != null)
        {
            menuActive.SetActive(false);
        }
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
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(winButtonSelect);
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
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(loseButtonSelect);
    }

    public void AddPoints(float amount)
    {
        Points += amount;
        GameManager.instance.player.GetComponent<LevelSystem>().CheckForLevelUp();
    }

    private string SavePath => Path.Combine(Application.persistentDataPath, "savegame.json");

    public void SaveGame(float score, float health, Vector3 pos)
    {
        PlayerData playerStats = new PlayerData
        {
            playerHealth = health,
            playerScore = score,
            playerPos = pos
        };
        // 1. Convert the data object to a JSON string representation
        string json = JsonUtility.ToJson(playerStats, true);

        // 2. Write that string onto the user's hard drive 
        File.WriteAllText(SavePath, json);
        Debug.Log($"Game Saved to: {SavePath}");
    }

    public PlayerData LoadGame()
    {
        // Check if a save file actually exists before trying to read it
        if (File.Exists(SavePath))
        {
            // 1. Read the raw text from the file
            string json = File.ReadAllText(SavePath);

            // 2. Overwrite the runtime object with the saved values
            PlayerData playerStats = JsonUtility.FromJson<PlayerData>(json);
            Debug.Log("Game Loaded Successfully!");
            return playerStats;
        }
        else
        {
            Debug.LogWarning("No save file found. Starting fresh!");
            return new PlayerData { playerHealth = 100f, playerScore = 0f, playerPos = Vector3.zero };
        }
    }


}
