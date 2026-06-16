using UnityEngine;
using UnityEngine.SocialPlatforms;

public class BossDoublePunch : MonoBehaviour
{

    enum PunchStates { AtRest = 0, PunchForward = 1, AtPoint = 2, Returning = 3, WaitingForLaunch = 4}
    [Header("Part Setup")]
    [SerializeField] CharacterController controller;
    [SerializeField] int difficultyMod;
    [SerializeField] AudioSource myAudio;
    [SerializeField] AudioClip[] mySounds;
    public bool isActive;


    [Header("Attack Basics")]
    [SerializeField] Vector3 pointToReach;
    [SerializeField] GameObject warningSign;
    [SerializeField] float timeToReachPoint;
    [SerializeField] float timeToReturnPoint;
    [SerializeField] float timeToWaitAtTarget;
    [SerializeField] float timeToShowWarningFirst;
    [SerializeField] float timeToShowWarningRepeat;
    [SerializeField] int numPunchesToDo;
    [SerializeField] float timeBetweenPunches;
    [SerializeField] GameObject otherFist;

    [Header("Visual Effects")]
    [SerializeField] LineRenderer armLine;

    [SerializeField] PunchStates currentState;

    int baseNumPunchesToDo;
    float baseTimeToReachPoint;
    float baseTimeBetweenPunches;

    float punchMoveSpeed;



    int numPunchesCompleted;


    float waitForLaunchTime;
    float waitReachPointTime;
    float waitAtTargetTime;
    float waitReturnHomeTime;

    Vector3 myInitDirection;
    Vector3 homePoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        baseTimeToReachPoint = timeToReachPoint;
        baseNumPunchesToDo = numPunchesToDo;
        baseTimeBetweenPunches = timeBetweenPunches;

        homePoint = transform.position;
       

        ResetValues();
        
    }

    // Update is called once per frame
    void Update()
    {
        DrawLine();

        CheckIfAttacking();

        if(isActive)
        {
            SynchronizeFists();

            switch (currentState)
            {
                case PunchStates.AtRest:

                    IsAttackDone();
                    if (isActive)
                    {
                        SetupLaunch();
                    }

                    break;

                case PunchStates.WaitingForLaunch:

                    waitForLaunchTime += Time.deltaTime;
                    if(numPunchesCompleted == 0)
                    {
                        if(waitForLaunchTime >= timeToShowWarningFirst)
                        {
                            LaunchFist();
                        }
                    }
                    else if (waitForLaunchTime >= timeToShowWarningRepeat)
                    {
                        LaunchFist();
                    }

                        break;

                case PunchStates.PunchForward:

                    waitReachPointTime += Time.deltaTime;
                    MoveFist();
                    if(waitReachPointTime >= timeToReachPoint)
                    {
                        currentState = PunchStates.AtPoint;
                        waitReachPointTime = 0;
                    }

                    break;

                case PunchStates.AtPoint:

                    WaitAtTarget();

                    break;


                case PunchStates.Returning:

                    waitReturnHomeTime += Time.deltaTime;
                    MoveFistHome();
                    if(waitReturnHomeTime >= timeToReturnPoint)
                    {
                        currentState = PunchStates.AtRest;
                        controller.enabled = false;
                        transform.position = homePoint;
                        controller.enabled = true;
                        waitReturnHomeTime = 0;
                        numPunchesCompleted += 1;
                    }

                    break;

                default:
                    break;
            }
        }
    }

    void DrawLine()
    {
        if (armLine != null)
        {
            armLine.SetPosition(0, homePoint);
            armLine.SetPosition(1, transform.position);
        }
    }

    void CheckIfAttacking()
    {
        if (gameObject.GetComponent<BossPart>().attackingRightNow == true)
        {
            isActive = true;
        }
    }

    void SetupLaunch()
    {

        pointToReach = GameManager.instance.player.transform.position;
        GameObject myWarning = Instantiate(warningSign, pointToReach, Quaternion.identity);
        if (numPunchesCompleted == 0)
        {
            myWarning.GetComponent<DestroySelfAfterTime>().lifetime = timeToShowWarningFirst;
        }
        else
        {
            myWarning.GetComponent<DestroySelfAfterTime>().lifetime = timeToShowWarningRepeat;
        }
            currentState = PunchStates.WaitingForLaunch;
        waitForLaunchTime = 0;

    }

    void LaunchFist()
    {
        TargetPoint();
        currentState = PunchStates.PunchForward;
    }

    public void TargetPoint()
    {
        myInitDirection = pointToReach - gameObject.transform.position;
        Quaternion rot = Quaternion.LookRotation(new Vector3(myInitDirection.x, 0, myInitDirection.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, 1f);


        punchMoveSpeed = (Vector3.Distance(transform.position, pointToReach)) / timeToReachPoint;
        


    }

    void TargetHome()
    {
        punchMoveSpeed = (Vector3.Distance(transform.position, homePoint)) / timeToReturnPoint;
    }

    void MoveFist()
    {
        controller.Move(transform.forward * punchMoveSpeed * Time.deltaTime);
    }

    void MoveFistHome()
    {
        controller.Move(-transform.forward * punchMoveSpeed * Time.deltaTime);
    }

    void WaitAtTarget()
    {
        waitAtTargetTime += Time.deltaTime;
        if(waitAtTargetTime >= timeToWaitAtTarget)
        {
            currentState = PunchStates.Returning;
            TargetHome();
        }
    }

    void IsAttackDone()
    {
        if (numPunchesCompleted >= numPunchesToDo)
        {
            EndAttack();
        }
    }

    void EndAttack()
    {
        gameObject.GetComponent<BossPart>().attackingRightNow = false;
        isActive = false;
        ResetValues();
        SynchronizeFists();
    }

    void SynchronizeFists()
    {
        if(otherFist != null)
        {
            otherFist.GetComponent<BossPart>().attackingRightNow = gameObject.GetComponent<BossPart>().attackingRightNow;
        }
    }

    void ResetValues()
    {

        numPunchesToDo = baseNumPunchesToDo;
        timeToReachPoint = baseTimeToReachPoint;
        timeBetweenPunches = baseTimeBetweenPunches;
        numPunchesCompleted = 0;

        currentState = PunchStates.AtRest;
        waitForLaunchTime = 0;
        waitReachPointTime = 0;
        waitAtTargetTime = 0;
        waitReturnHomeTime = 0;


        difficultyMod = gameObject.GetComponent<BossPart>().difficultyMod;

        switch (difficultyMod)
        {
            case 1:
                break;

            case 2:
                numPunchesToDo = (int)(numPunchesToDo * 1.5);
                timeToReachPoint = (float)(timeToReachPoint / 1.25);
                timeBetweenPunches = (float)(timeBetweenPunches * 1.2);

                break;
            case 3:
                numPunchesToDo = (int)(numPunchesToDo * 2);
                timeToReachPoint = (float)(timeToReachPoint / 1.5);
                timeBetweenPunches = (float)(timeBetweenPunches * 1.4);
                break;

            default:
                break;
        }

    }
}

