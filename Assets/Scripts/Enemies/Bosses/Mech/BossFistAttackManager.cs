using UnityEngine;

public class BossFistAttackManager : MonoBehaviour
{

    public bool attackChosen;
    [SerializeField] float myChoice;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        attackChosen = false;
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfAttacking();
    }

    void RandomAttackChoice()
    {

        myChoice = Random.Range(0f, 1f);

        if(myChoice >= 0.5)
        {
            gameObject.GetComponent<BossDoublePunch>().isActive = true;
        }
        else
        {
            gameObject.GetComponent<BossSwipeAttack>().isActive = true;
        }


        gameObject.GetComponent<BossSwipeAttack>().GetOtherFist().GetComponent<BossFistAttackManager>().attackChosen = true;
        attackChosen = true;
    }

    void CheckIfAttacking()
    {
        if (gameObject.GetComponent<BossPart>().attackingRightNow == true && attackChosen == false)
        {
          
                RandomAttackChoice();
            
            
        }
    }

}
