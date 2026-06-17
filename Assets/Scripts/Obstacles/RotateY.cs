using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] int speed;
    void Update()
    {
        transform.Rotate(Vector3.up, Time.deltaTime * speed);
    }
}
