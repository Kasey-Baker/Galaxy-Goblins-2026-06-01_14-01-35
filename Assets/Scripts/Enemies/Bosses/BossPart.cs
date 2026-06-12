using System.Linq;
using UnityEngine;

public class BossPart : MonoBehaviour
{
    [SerializeField] GameObject owner;
    public int difficultyMod;

    public bool attackingRightNow;


    private void Awake()
    {
        difficultyMod = owner.GetComponent<BossManager>().difficultyModifer;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        attackingRightNow = false;
      
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
