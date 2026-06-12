using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerControls : MonoBehaviour, IDamage
{
    [SerializeField] float healthCurr;
    [SerializeField] CharacterController control;
    [SerializeField] float moveSpeed;
    [SerializeField] float slowSpeedMod;

    [SerializeField] float firerate;
    [SerializeField] GameObject[] bulletList;
    Vector3 moveDirection;

    float healthMax;

    float fireWait;

    int currBullet;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthMax = healthCurr;
        currBullet = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();

        /*
        fireWait += Time.deltaTime;

        if(fireWait >= firerate && bulletList[currBullet] != null)
        {
            ShootBasic(bulletList[currBullet], 0);
        }
        */

    }

    void ShootBasic(GameObject bullet, float angleMod)
    {
        if (Input.GetButton("Fire1"))
        {
            Instantiate(bullet, transform.position, Quaternion.Euler(0f, angleMod, 0f));
            fireWait = 0;
        }
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
        updatePlayerUI();
        if (healthCurr <= 0)
        {
            GameManager.instance.YouLose();
        }

        if(healthCurr > healthMax)
        {
            healthCurr = healthMax;
        }
    }

    public void updatePlayerUI()
    {
        GameManager.instance.playerHPBar.fillAmount = healthCurr / healthMax;
    }
    /*
        public void changePlayerPos()
        {
            control.transform.position = GameManager.instance.playerStartPos.transform.position;
            Physics.SyncTransforms();
            healthCurr = healthMax;
            updatePlayerUI();
        }
    */

   public void ApplyEffects(ItemData item)
    {
        if (item.healthBonus != 0)
        {
            healthMax += item.healthBonus;
            healthCurr += item.healthBonus;
            updatePlayerUI();
        }
        if (item.speedBonus != 0)
        {
            moveSpeed += item.speedBonus;
        }
        if (item.speedMultiplier != 0)
        {
            moveSpeed *= item.speedMultiplier;
        }
        if (item.firerateMultiplier != 0)
        {
            firerate /= item.firerateMultiplier;
        }
    }




}
