
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyBasicBehavior : MonoBehaviour
{ 


 

    GameObject player;

    [SerializeField] GameObject[] spriteOptions;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for(int i = 0; i < spriteOptions.Length; i++)
        {
            spriteOptions[i].SetActive(false);
        }
        player = GameManager.instance.player; //Replace this with gamemanager player when gamemanager implemented

        if(spriteOptions.Length > 1)
        {
            switch(SceneManager.GetActiveScene().name)
            {
                case "Grass Level":

                    spriteOptions[0].SetActive(true);

                    break;

                case "Water Level":

                    spriteOptions[1].SetActive(true);

                    break;

                case "Volcano Level":

                    spriteOptions[2].SetActive(true);

                    break;

                default:

                    spriteOptions[3].SetActive(true);

                break;
            }
            if(SceneManager.GetActiveScene().name == "Grass Level")
            {

            }
        }
        else
        {
            spriteOptions[0].SetActive(true);
        }

    }

    // Update is called once per frame
    void Update()
    {

        float distance = Vector3.Distance(gameObject.transform.position, player.transform.position);
        if (Mathf.Abs(distance) > 50)
        {
            Destroy(gameObject);
        }
    }
}
