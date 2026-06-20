using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class BossPart : MonoBehaviour
{
    [SerializeField] GameObject owner;
    [SerializeField] AudioSource myAudio;
    [SerializeField] AudioClip[] deathSounds;
    public int difficultyMod;

    public bool attackingRightNow;


    private void Awake()
    {
       
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //difficultyMod = owner.GetComponent<BossManager>().difficultyModifer;
        attackingRightNow = false;
    
      
    }

    public void UpdateHealth()
    {
        float newHP;
        newHP = gameObject.GetComponent<EnemyTakeDamage>().GetHealth();
        switch (difficultyMod)
        {

            case 1:


                newHP *= 1;
                gameObject.GetComponent<EnemyTakeDamage>().SetHealth(newHP);

                break;

            case 2:

                newHP *= 4;
                gameObject.GetComponent<EnemyTakeDamage>().SetHealth(newHP);

                break;

            case 3:

                newHP *= 9;
                gameObject.GetComponent<EnemyTakeDamage>().SetHealth(newHP);

                break;

            default:

                break;



        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        if (owner.GetComponent<BossManager>())
        {
            for (int i = 0; i < owner.GetComponent<BossManager>().bossParts.Count; i++)
            {
                if (owner.GetComponent<BossManager>().bossParts[i] == gameObject)
                {
                    owner.GetComponent<BossManager>().bossParts.Remove(gameObject);
                    owner.GetComponent<BossManager>().ModifyAttackSpeed(0.75f);

                    if(owner.GetComponent<BossManager>().bossParts.Count == 0)
                    {
                        Destroy(owner);
                    }
                }

            }
        }

    }

    public void AttackActivate()
    {
        attackingRightNow = true;
    }

    public void AttackDisable()
    {
        attackingRightNow = false;
    }

    public GameObject GetOwner()
    {
        return owner;
    }

   

}
