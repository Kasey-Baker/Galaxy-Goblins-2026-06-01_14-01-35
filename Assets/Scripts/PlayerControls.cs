using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerControls : MonoBehaviour, IDamage
{
    [SerializeField] float healthCurr;
    [SerializeField] CharacterController control;
    [SerializeField] float moveSpeed;
    [SerializeField] float slowSpeedMod;
    Vector3 moveDirection;

    float healthMax;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthMax = healthCurr;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        moveDirection = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;
        control.Move(moveDirection * moveSpeed * SlowMove() * Time.deltaTime);
    }

    float SlowMove()
    {
        if(Input.GetButton("SlowMove"))
        {
            return slowSpeedMod;
        }
        else 
        {
            return 1;
        }
    }

    public void takeDamage(float amount)
    {
        healthCurr -= amount;
    }
}
