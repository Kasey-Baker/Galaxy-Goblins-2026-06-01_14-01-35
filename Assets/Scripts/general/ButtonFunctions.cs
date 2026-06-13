using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    public void resume()
    {
        GameManager.instance.stateUnpaused();
    }

    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameManager.instance.stateUnpaused();
        if(GameManager.instance.player != null)
        {
            Destroy(GameManager.instance.player);
        }
    }

    public void quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
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
}
