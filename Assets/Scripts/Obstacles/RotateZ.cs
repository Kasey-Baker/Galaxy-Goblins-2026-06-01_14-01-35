using UnityEngine;

public class RotateZ : MonoBehaviour
{
    [SerializeField] int speed;
    void Update()
    {
        transform.Rotate(Vector3.right, Time.deltaTime * speed);
    }
}
