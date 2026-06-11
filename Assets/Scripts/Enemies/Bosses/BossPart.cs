using System.Linq;
using UnityEngine;

public class BossPart : MonoBehaviour
{
    [SerializeField] GameObject owner;
    [SerializeField] int difficultyMod;
    [SerializeField] Component attackToActivate;

    public bool attackingRightNow;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        attackingRightNow = false;
        difficultyMod = owner.GetComponent<BossManager>().difficultyModifer;
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

}
