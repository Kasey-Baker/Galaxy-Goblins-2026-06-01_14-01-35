using UnityEditor.UI;
using UnityEngine;

public class EnemyBasicBehavior : MonoBehaviour
{ 


 

    GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        player = GameObject.FindWithTag("Player"); //Replace this with gamemanager player when gamemanager implemented
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
