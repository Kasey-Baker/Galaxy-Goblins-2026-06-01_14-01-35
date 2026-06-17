using UnityEngine;

public class BossSwipeAttack : MonoBehaviour
{

    [SerializeField] CharacterController controller;
    [SerializeField] int numSwipes;
    [SerializeField] float swipeSpeed;
    [SerializeField] GameObject[] swipePathPoints;
    [SerializeField] float timeBetweenSwipes;
    [SerializeField] GameObject otherFist;

    [SerializeField] int difficultyMod;
    [SerializeField] AudioSource myAudio;
    [SerializeField] AudioClip[] mySounds;

    [SerializeField] LineRenderer warningLine;

    public bool isActive;


    bool isTraveling;

    bool doneStartup;
    public bool isCurrentSwiper;

    int baseNumSwipes;
    float baseSwipeSpeed;

    float waitTime;
    float numSwipesCompleted;

    Vector3 homePoint;
    Vector3 currPointToReach;
    Vector3 myInitDirection;

    Vector3 moveDir;

    int currPointToReachIndex;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        baseNumSwipes = numSwipes;
        baseSwipeSpeed = swipeSpeed;
        homePoint = transform.position;
        doneStartup = false;
        ResetValues();
        warningLine = gameObject.GetComponent<BossPart>().GetOwner().GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        SafetyCheck();
        if (isActive)
        {
            gameObject.GetComponent<BossPart>().attackingRightNow = true;
            if (doneStartup == false)
            {
                OtherFistStartup();
                doneStartup = true;
                numSwipesCompleted = 0;
            }
        }
        CheckIfAttacking();
        if (isActive && isCurrentSwiper)
        {
            if (isTraveling == false)
            {
                SetTravelToNextPoint();
                DrawWarning();
            }
            else
            {
                MoveFist();
                IsCloseToPoint();
            }
        }
    }

    void DrawWarning()
    {
        warningLine.enabled = true;
        warningLine.SetPosition(0, homePoint);
        for(int i = 0; i < swipePathPoints.Length; i++)
        {
            warningLine.SetPosition(i + 1, swipePathPoints[i].transform.position);
        }
    }

    public GameObject GetOtherFist()
    {
        return otherFist;
    }

    void SetTravelToNextPoint()
    {
        
        if (currPointToReachIndex < swipePathPoints.Length)
        {
            currPointToReach = swipePathPoints[currPointToReachIndex].transform.position;
            currPointToReachIndex += 1;
        }
        else
        {
            currPointToReach = homePoint;
        }
        myInitDirection = currPointToReach - gameObject.transform.position;
        Quaternion rot = Quaternion.LookRotation(new Vector3(myInitDirection.x, 0, myInitDirection.z));

        Quaternion prevRotation = transform.rotation;
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, 1f);
        moveDir = transform.forward;
        if(currPointToReach == homePoint)
        {
            transform.forward = -transform.forward;
        }
        isTraveling = true;

    }

    void SafetyCheck()
    {
        if (gameObject.GetComponent<BossDoublePunch>().isActive)
        {
            isActive = false;
            //ResetValues();
            //EndAttack();
        }
    }

    void IsCloseToPoint()
    {
        if (Vector3.Distance(currPointToReach, transform.position) <= 2)
        {
            isTraveling = false;
            if(currPointToReach == homePoint)
            {
                numSwipesCompleted += 1;
                currPointToReachIndex = 0;
                controller.enabled = false;
                transform.position = homePoint;
                controller.enabled = true;
                IsAttackDone();
                SynchronizeFists();
            }
            
        }
    }

    void CheckIfAttacking()
    {
        if (gameObject.GetComponent<BossPart>().attackingRightNow == true)
        {
            //isActive = true;
        }
    }

    void IsAttackDone()
    {
        if (numSwipesCompleted >= numSwipes)
        {
            EndAttack();
        }
    }

    void EndAttack()
    {
        gameObject.GetComponent<BossPart>().attackingRightNow = false;
        gameObject.GetComponent<BossFistAttackManager>().attackChosen = false;
        isActive = false;
        warningLine.enabled = false;
        ResetValues();
    }

    void MoveFist()
    {
        controller.Move(moveDir * swipeSpeed * Time.deltaTime);
    }

    void OtherFistStartup()
    {
        if(otherFist != null)
        {
            if(otherFist.GetComponent<BossSwipeAttack>().isActive == false)
            {
                otherFist.GetComponent<BossSwipeAttack>().isActive = true;
                otherFist.GetComponent<BossSwipeAttack>().isCurrentSwiper = false;
                isCurrentSwiper = true;
            }
        }
        else if(otherFist == null)
        {
            isCurrentSwiper = true;
        }
    }

    void SynchronizeFists()
    {
        if(otherFist != null)
        {
            otherFist.GetComponent<BossSwipeAttack>().isCurrentSwiper = true;
            isCurrentSwiper = false;
        }
    }

    void ResetValues()
    {
        doneStartup = false;
        isTraveling = false;
        isCurrentSwiper = false;
        numSwipesCompleted = 0;
        waitTime = 0;

        numSwipes = baseNumSwipes;
        swipeSpeed = baseSwipeSpeed;

        currPointToReachIndex = 0;

        difficultyMod = gameObject.GetComponent<BossPart>().difficultyMod;

        switch (difficultyMod)
        {
            case 1:
                break;

            case 2:
                numSwipes = (int)(numSwipes * 2);
                swipeSpeed = (float)(swipeSpeed * 1.5);

                break;
            case 3:
                numSwipes = (int)(numSwipes * 3);
                swipeSpeed = (float)(swipeSpeed * 2);
                break;

            default:
                break;
        }
    }

}
