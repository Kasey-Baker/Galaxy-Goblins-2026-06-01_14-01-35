using UnityEngine;
using UnityEngine.InputSystem.XR;

public class TentacleAttack : MonoBehaviour
{
    [Header("Behavior Setup")]
    [SerializeField] CharacterController controller;
    [SerializeField] GameObject warningSign;
    [SerializeField] Transform upperBound;
    [SerializeField] Transform lowerBound;

    [Header("Attack Difficult Modifiers")]
    [SerializeField] float moveSpeedVert;
    [SerializeField] float moveSpeedHoriz;
    [SerializeField] int numRepetitions;
    [SerializeField] float timeBetweenLunges;
    [SerializeField] float lungeSpeed;
    [SerializeField] float warningDisplayTime;
    [SerializeField] float waitAtPointTime;
    [SerializeField] int difficultyMod;

    [Header("Visuals And Audio")]
    [SerializeField] AudioSource myAudio;
    [SerializeField] AudioClip[] mySounds;
    [SerializeField] LineRenderer warningLine;

    public bool isActive;

    float baseTimeBetweenLunges;
    int baseNumRepetitions;
    float baseLungeSpeed;
    float baseWarningDisplayTime;
    float baseWaitAtPointTime;
    float baseMoveSpeedHoriz;
    float baseMoveSpeedVert;

    Vector3 homePoint;
    Vector3 pointToReach;
    Vector3 myInitDirection;

    float waitTime;
    float repsCompleted;
    enum AttackStates { AtRest = 0, FindRandomPoint = 1, DisplayWarning = 2, WaitForLunge = 3, TravelToPointVert = 4, LungeToPointHoriz = 5, WaitAtPoint = 6, ReturnHomeVert = 7, AtHomeVert = 8, ReturnHomeHoriz = 9}
    [SerializeField] AttackStates currState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        warningLine = gameObject.GetComponent<BossPart>().GetOwner().GetComponent<LineRenderer>();
        homePoint = transform.position;
        currState = AttackStates.AtRest;

        baseTimeBetweenLunges = timeBetweenLunges;
        baseNumRepetitions = numRepetitions;
        baseLungeSpeed = lungeSpeed;
        baseWarningDisplayTime = warningDisplayTime;
        baseWaitAtPointTime = waitAtPointTime;
        baseMoveSpeedHoriz = moveSpeedHoriz;
        baseMoveSpeedVert = moveSpeedVert;
        ResetValues();
    }

    // Update is called once per frame
    void Update()
    {
        SafetyCheck();
        if(isActive)
        {
            switch(currState)
            {
                case AttackStates.AtRest:

                    IsAttackDone();
                    if(isActive)
                    {
                        currState = AttackStates.FindRandomPoint;
                    }

                    break;

                case AttackStates.FindRandomPoint:

                    FindRandomPoint();

                    break;

                case AttackStates.TravelToPointVert:

                    TravelToPointVert();
                    IsCloseToPointVert();

                    break;

                case AttackStates.DisplayWarning:

                    CreateWarning();

                    waitTime = 0;
                    currState = AttackStates.WaitForLunge;

                    break;

                case AttackStates.WaitForLunge:

                    waitTime += Time.deltaTime;
                    if(waitTime >= warningDisplayTime)
                    {
                        currState = AttackStates.LungeToPointHoriz;
                        if (mySounds.Length > 0)
                        {
                            myAudio.PlayOneShot(mySounds[Random.Range(0, mySounds.Length)]);
                        }
                    }

                    break;

                case AttackStates.LungeToPointHoriz:

                    TravelToPointHoriz();
                    IsCloseToPointHoriz();

                    break;

                case AttackStates.WaitAtPoint:

                    waitTime += Time.deltaTime;
                    if(waitTime >= waitAtPointTime)
                    {
                        currState = AttackStates.ReturnHomeHoriz;
                    }

                    break;

                case AttackStates.ReturnHomeVert:

                    TravelToHomeVert();
                    IsCloseToHomeVert();

                    break;


                case AttackStates.ReturnHomeHoriz:

                    TravelToHomeHoriz();
                    IsCloseToHomeHoriz();

                    break;


            }
        }
    }

    void SafetyCheck()
    {
        if (gameObject.GetComponent<AsteroidThrow>().isActive)
        {
            isActive = false;
        }
    }

    void FindRandomPoint()
    {
        float zPosition = Random.Range(upperBound.position.z, lowerBound.position.z);
        pointToReach = new Vector3(upperBound.position.x, homePoint.y, zPosition);
        currState = AttackStates.TravelToPointVert;
    }


    //Travelling to lunge point and back
    #region
    void TravelToPointVert()
    {
        myInitDirection = pointToReach - gameObject.transform.position;
        myInitDirection = new Vector3(0, 0, myInitDirection.z);
        Quaternion rot = Quaternion.LookRotation(new Vector3(0, 0, myInitDirection.z));
        controller.Move(myInitDirection.normalized * moveSpeedVert * Time.deltaTime);
    }


    void TravelToPointHoriz()
    {
        myInitDirection = pointToReach - gameObject.transform.position;
        myInitDirection = new Vector3(myInitDirection.x, 0, 0);
        Quaternion rot = Quaternion.LookRotation(new Vector3(myInitDirection.x, 0, 0));
        controller.Move(myInitDirection.normalized * lungeSpeed * Time.deltaTime);
    }

    void IsCloseToPointVert()
    {
        if (Mathf.Abs(pointToReach.z - transform.position.z) <= 1)
        {
            currState = AttackStates.DisplayWarning;
            controller.enabled = false;
            transform.position = new Vector3(transform.position.x, transform.position.y, pointToReach.z);
            controller.enabled = true;
        }
    }

    void IsCloseToPointHoriz()
    {
        if (Mathf.Abs(pointToReach.x - transform.position.x) <= 1)
        {
            currState = AttackStates.WaitAtPoint;
            controller.enabled = false;
            transform.position = new Vector3(pointToReach.x, transform.position.y, transform.position.z);
            controller.enabled = true;
            warningLine.enabled = false;
            waitTime = 0;
        }
    }

    void TravelToHomeVert()
    {
        myInitDirection = homePoint - gameObject.transform.position;
        myInitDirection = new Vector3(0, 0, myInitDirection.z);
        Quaternion rot = Quaternion.LookRotation(new Vector3(0, 0, myInitDirection.z));
        controller.Move(myInitDirection.normalized * moveSpeedVert * Time.deltaTime);
    }

    void TravelToHomeHoriz()
    {
        myInitDirection = homePoint - gameObject.transform.position;
        myInitDirection = new Vector3(myInitDirection.x, 0, 0);
        Quaternion rot = Quaternion.LookRotation(new Vector3(myInitDirection.x, 0, 0));
        controller.Move(myInitDirection.normalized * moveSpeedHoriz * Time.deltaTime);
    }

    void IsCloseToHomeVert()
    {
        if (Mathf.Abs(homePoint.z - transform.position.z) <= 1)
        {
            currState = AttackStates.AtRest;
            controller.enabled = false;
            transform.position = new Vector3(transform.position.x, transform.position.y, homePoint.z);
            controller.enabled = true;
        }
    }

    void IsCloseToHomeHoriz()
    {
        if (Mathf.Abs(homePoint.x - transform.position.x) <= 1)
        {
            repsCompleted += 1;
            if (repsCompleted >= numRepetitions)
            {
                currState = AttackStates.ReturnHomeVert;
                controller.enabled = false;
                transform.position = new Vector3(homePoint.x, transform.position.y, transform.position.z);
                controller.enabled = true;
            }
            else
            {
                currState = AttackStates.FindRandomPoint;
                controller.enabled = false;
                transform.position = new Vector3(homePoint.x, transform.position.y, transform.position.z);
                controller.enabled = true;
            }
        }
    }
    #endregion

    void CheckIfAttacking()
    {
        if (gameObject.GetComponent<BossPart>().attackingRightNow == true)
        {
            isActive = true;
        }
    }

    void CreateWarning()
    {
        warningLine.enabled = true;
        warningLine.SetPosition(0, transform.position);
        warningLine.SetPosition(1, pointToReach);
    }

    void IsAttackDone()
    {
        if (repsCompleted >= numRepetitions)
        {
            EndAttack();
        }
    }

    void EndAttack()
    {
        gameObject.GetComponent<BossPart>().attackingRightNow = false;
        gameObject.GetComponent<BossTentacleAttackManager>().attackChosen = false;
        isActive = false;
        ResetValues();
    }

    void ResetValues()
    {
        repsCompleted = 0;
        waitTime = 0;

        numRepetitions = baseNumRepetitions;
        timeBetweenLunges = baseTimeBetweenLunges;
        lungeSpeed = baseLungeSpeed;
        moveSpeedVert = baseMoveSpeedVert;
        moveSpeedHoriz = baseMoveSpeedHoriz;
        warningDisplayTime = baseWarningDisplayTime;
        waitAtPointTime = baseWaitAtPointTime;
        

        difficultyMod = gameObject.GetComponent<BossPart>().difficultyMod;

        switch (difficultyMod)
        {
            case 1:
                break;

            case 2:
                timeBetweenLunges = (float)(timeBetweenLunges * 0.9);
                numRepetitions = (int)(numRepetitions * 1.5);
                moveSpeedHoriz = (float)(moveSpeedHoriz * 1.5);
                moveSpeedVert = (float)(moveSpeedVert * 1.5);
                warningDisplayTime = (float)(warningDisplayTime * 0.9f);
                waitAtPointTime = (float)(waitAtPointTime * 0.9f);
                lungeSpeed = (float)(lungeSpeed * 1.2);
                

                break;
            case 3:
                timeBetweenLunges = (float)(timeBetweenLunges * 0.8);
                numRepetitions = (int)(numRepetitions * 2);
                moveSpeedHoriz = (float)(moveSpeedHoriz * 2);
                moveSpeedVert = (float)(moveSpeedVert * 2);
                warningDisplayTime = (float)(warningDisplayTime * 0.8f);
                waitAtPointTime = (float)(waitAtPointTime * 0.8f);
                lungeSpeed = (float)(lungeSpeed * 1.4);
                break;

            default:
                break;
        }
    }
}