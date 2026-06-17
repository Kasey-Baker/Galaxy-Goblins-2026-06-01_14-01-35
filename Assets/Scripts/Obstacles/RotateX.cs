using UnityEngine;

public class RotateX : MonoBehaviour
{
    [SerializeField] int speed;
    void Update()
    {
        transform.Rotate(Vector3.forward, Time.deltaTime * speed);
    }
}
