using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (GameManager.instance != null)
        {
            if(GameManager.instance.player != null)
            {
                GameManager.instance.playercontrols.GoToSpawn();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
