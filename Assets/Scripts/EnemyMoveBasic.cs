using UnityEngine;

public class EnemyMoveBasic : MonoBehaviour
{
    [SerializeField] CharacterController controller;
    [SerializeField] float moveSpeed;

    Vector3 moveDir;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveDir = new Vector3(0, 0, moveSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        controller.Move(moveDir * Time.deltaTime);
    }
}
