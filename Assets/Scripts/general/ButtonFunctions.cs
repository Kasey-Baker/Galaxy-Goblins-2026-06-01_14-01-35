using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using UnityEngine.UI;

public class ButtonFunctions : MonoBehaviour
{
    [SerializeField] private GameObject loadMenu;
    [SerializeField] GameObject selectSfxPlayer;
    [SerializeField] AudioClip selectSfxToPlay;
    private Button quitbutton;
    public void resume()
    {
        GameManager.instance.stateUnpaused();
    }

    public void restart()
    {
        SceneManager.LoadScene("Tutorial");
        GameManager.instance.stateUnpaused();
        if(GameManager.instance.player != null)
        {
            Destroy(GameManager.instance.player);
        }
    }

    public void quit()
    {

#if UNITY_WEBGL
        quitbutton.interactable = false;
        return;
#endif
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        quitbutton = GetComponent<Button>();

    }

    public void playerRespawn()
    {
       // GameManager.instance.playercontrols.changePlayerPos();
        GameManager.instance.stateUnpaused();
    }

    public void loadLevel(int lvl)
    {
        SceneManager.LoadScene(lvl);
        GameManager.instance.stateUnpaused();
    }

    public void LoadLevelName(string levelName)
    {
        //StartCoroutine(LoadMenu(levelName));

        //loadMenu.SetActive(true);
        Instantiate(loadMenu);
        SceneManager.LoadScene(levelName);
        if (GameManager.instance != null)
        {
            GameManager.instance.stateUnpaused();
        }
    }

    public void MainMenu(int lvl)
    {
        SceneManager.LoadScene(lvl);
        GameManager.instance.stateUnpaused();
    }

    public void LoadGame()
    {
        if(SaveSystem.instance != null)
        {
            SaveSystem.instance.LoadGame();
        }
    }

    public void SaveGame()
    {
        if(SaveSystem.instance != null)
        {
            SaveSystem.instance.SaveGame();
        }
    }

    IEnumerator LoadMenu(string levelName)
    {
        loadMenu.SetActive(true);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelName);
        asyncLoad.allowSceneActivation = false;
        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
            {
                yield return new WaitForSeconds(1f);
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }
        
    }


}
