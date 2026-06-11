using UnityEngine;

public class DestroySelfAfterTime : MonoBehaviour
{

    [SerializeField] float lifetime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
