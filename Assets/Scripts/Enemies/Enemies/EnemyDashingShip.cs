using UnityEngine;
using UnityEngine.InputSystem.XR;

public class EnemyDashingShip : MonoBehaviour
{
    [SerializeField] CharacterController controller;
    [SerializeField] float moveSpeed;
    Vector3 moveDir;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveDir = transform.forward * moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        moveDir = transform.forward * moveSpeed;
        controller.Move(moveDir * Time.deltaTime);
    }

    public void SetMoveSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }
}
