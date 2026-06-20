using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class PlayerControls : MonoBehaviour, IDamage
{
    [Header("Player Config")]
    [SerializeField] CharacterController control;
    [SerializeField] GameObject playerSprite;
    [SerializeField] GameObject[] afterEffects;
    [SerializeField] Transform[] afterEffectsBasePos;
    [SerializeField] float invulnTimeOnHit;
    [SerializeField] bool isInvuln;

    [Header("Visual Effects")]
    [Range(0f, 10f)] [SerializeField] float afterEffectMod;
    [Range(0f, 5f)] [SerializeField] float baseOffset;
    [Range(0f, 45f)] [SerializeField] float tiltAmount;


    Color colorOrig;

    Renderer[] allRenders;
    Color[] allColors;
    [SerializeField] Renderer rend;

    [Header("Player Stats")]
    [SerializeField] float healthCurr;
    [SerializeField] float moveSpeed;
    [SerializeField] float slowSpeedMod;
    [SerializeField] float firerate;

    [Header("Player Bullets")]
    [SerializeField] GameObject[] bulletList;
    [SerializeField] GameObject bigBullet;
    [SerializeField] GameObject FastBullet;
    [SerializeField] GameObject FastBigBullet;

    [Header("Audio")]
    [SerializeField] AudioSource myAudio;
    [SerializeField] AudioClip[] myHitSounds;

    Vector3 moveDirection;

    float healthMax;

    float invulnWait;

    int currBullet;
    float spriteBaseX;
    float spriteBaseY;
    float spriteBaseZ;

    bool bigState;
    bool fastState;
    bool passiveRegenState;
    float regentimer;
    bool enemySlow;
    private void Awake()
    {
        if (GameManager.instance != null)
        {
            if (GameManager.instance.player == null)
            {
                GameManager.instance.player = gameObject;
                GameManager.instance.playercontrols = gameObject.GetComponent<PlayerControls>();
            }
        }

    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthMax = healthCurr;
        currBullet = 0;
        isInvuln = false;

        spriteBaseX = transform.rotation.x;
        spriteBaseY = transform.rotation.y;
        spriteBaseZ = transform.rotation.z;

        for(int i = 0; i < afterEffectsBasePos.Length; i++)
        {
            afterEffectsBasePos[i] = afterEffects[i].transform;
        }
        colorOrig = rend.material.color;
        allRenders = GetComponentsInChildren<Renderer>();
        allColors = new Color[allRenders.Length];
        for (int i = 0; i < allRenders.Length; i++)
        {
            if (allRenders[i].material.HasProperty("_Color"))
            {
                allColors[i] = (allRenders[i].material.color);
            }
        }


    }

    // Update is called once per frame
    void Update()
    {
        if(isInvuln)
        {
            invulnWait += Time.deltaTime;
            if(invulnWait >= invulnTimeOnHit)
            {
                isInvuln = false;
            }
        }

        Movement();
        passiveRegen();
        enemySlowdown();
        SelfRotation();


    }

    void ShootBasic(GameObject bullet, float angleMod)
    {
        if (Input.GetButton("Fire1"))
        {
            Instantiate(bullet, transform.position, Quaternion.Euler(0f, angleMod, 0f));
            //fireWait = 0;
        }
    }

    void Movement()
    {
        control.enabled = false;
        transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
        control.enabled = true;
        moveDirection = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;
        control.Move(moveDirection * moveSpeed * SlowMove() * Time.deltaTime);
    }
    void SelfRotation()
    {
        transform.rotation = new Quaternion(spriteBaseX, spriteBaseY, spriteBaseZ, 1);

        if (Mathf.Sign(Input.GetAxis("Horizontal")) == -1 && Input.GetAxis("Horizontal") != 0)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, tiltAmount);
        }
        else if (Mathf.Sign(Input.GetAxis("Horizontal")) == 1 && Input.GetAxis("Horizontal") != 0)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, playerSprite.transform.rotation.y, -tiltAmount);
        }

        if (Mathf.Sign(Input.GetAxis("Vertical")) == 1 && Input.GetAxis("Vertical") != 0)
        {
            transform.rotation = Quaternion.Euler(tiltAmount, transform.rotation.y, transform.rotation.z);
        }
        else if (Mathf.Sign(Input.GetAxis("Vertical")) == -1 && Input.GetAxis("Vertical") != 0)
        {
            transform.rotation = Quaternion.Euler(-tiltAmount, transform.rotation.y, transform.rotation.z);
        }
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

    IEnumerator flashOnHit()
    {
        if (rend != null)
        {
            rend.material.color = Color.red;
        }
        for (int i = 0; i < allRenders.Length; i++)
        {
            if (allRenders[i] != null)
            {
                allRenders[i].material.color = Color.white;
            }
        }
        yield return new WaitForSeconds(invulnTimeOnHit);
        rend.material.color = colorOrig;
        for (int i = 0; i < allRenders.Length; i++)
        {
            if (allRenders[i] != null)
            {
                allRenders[i].material.color = allColors[i];
            }
        }

    }

    void AfterImage()
    {
        float totalSpeed = (float)(moveSpeed * SlowMove() * afterEffectMod);
        for(int i = 0; i < afterEffects.Length; i++)
        {
            //afterEffects[i].transform.position = afterEffectsBasePos[i].transform.position;
            afterEffects[i].transform.position = new Vector3(afterEffects[i].transform.position.x, transform.position.y - (baseOffset * (i+1) * totalSpeed), afterEffects[i].transform.position.z);
            
            //afterEffects[i].transform.rotation = Quaternion.Euler(90, 0f, 0f);

        }
        //playerSprite.transform.position = new Vector3(transform.position.x, transform.position.y + (baseOffset * afterEffectMod * (3)), transform.position.z);
    }

    public void takeDamage(float amount)
    {
        if (!isInvuln || amount < 0)
        {
            healthCurr -= amount;
            if(amount > 0)
            {
                isInvuln = true;
                invulnWait = 0;
                StartCoroutine(flashOnHit());
            }
            if (myHitSounds.Length > 0)
            {
                myAudio.PlayOneShot(myHitSounds[Random.Range(0, myHitSounds.Length)]);
            }
            updatePlayerUI();
            if (healthCurr <= 0)
            {
                GameManager.instance.YouLose();
            }

            if (healthCurr > healthMax)
            {
                healthCurr = healthMax;
            }
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
        if (item.ItemEffect != ItemData.uniqueEffects.None)
        {
            if (item.ItemEffect == ItemData.uniqueEffects.BulletSize)
            {
                if (fastState)
                {
                    bulletList[0] = FastBigBullet;
                }
                else 
                { 
                    bulletList[0] = bigBullet; 
                }
                bigState = true;
            }
            if (item.ItemEffect == ItemData.uniqueEffects.PassiveRegen)
            {
                passiveRegenState = true;
            }
            if (item.ItemEffect == ItemData.uniqueEffects.BulletSpeed)
            {
                if (bigState)
                {
                    bulletList[0] = FastBigBullet;
                }
                else
                {
                    bulletList[0] = FastBullet;
                }
                fastState = true;
            }
            if (item.ItemEffect == ItemData.uniqueEffects.EnemySlowDown)
            {
                enemySlow = true;
            }
        }

    }

    private void passiveRegen()
    {
        if(passiveRegenState == true)
        {
            regentimer += Time.deltaTime;
            if (regentimer >= 5f)
            {
                healthCurr += (healthMax / 10f);
                regentimer = 0;

                if (healthCurr > healthMax)
                {
                    healthCurr = healthMax;
                }
                updatePlayerUI();
            }
            
        }
    }

    private void enemySlowdown()
    {
        if (enemySlow)
        {
            EnemyDashingShip[] Enemies = Object.FindObjectsByType<EnemyDashingShip>();

            foreach (EnemyDashingShip enemy in Enemies)
            {
                if (!enemy.isSlowed)
                {
                    float speed = enemy.GetMoveSpeed();
                    enemy.SetMoveSpeed(speed / 1.2f);

                    enemy.isSlowed = true;
                }
            }
        }
    }

    public void GoToSpawn()
    {
        if(GameObject.FindWithTag("PlayerSpawn") != null)
        {
            control.enabled = false;
            transform.position = GameObject.FindWithTag("PlayerSpawn").transform.position;
            control.enabled = true;
        }
    }
    


}
