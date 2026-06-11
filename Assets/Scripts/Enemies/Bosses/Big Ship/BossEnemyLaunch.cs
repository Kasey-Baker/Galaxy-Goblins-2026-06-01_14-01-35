using UnityEngine;
using UnityEngine.SocialPlatforms;

public class BossEnemyLaunch : MonoBehaviour
{
    [Header ("Part Setup")]
    [SerializeField] int difficultyMod;
    [SerializeField] AudioSource myAudio;
    [SerializeField] AudioClip[] mySounds;
    public bool isActive;


    [Header ("Spawn Basics")]
    [SerializeField] GameObject enemyToSpawn;
    [SerializeField] int numToSpawn;
    [SerializeField] int spawnReps;
    [SerializeField] float timeBetweenReps;

    //Spawn location finding stuff
    [Header ("Spawn Location Bounding")]
    [SerializeField] float distInFrontTargetZ; 
    [SerializeField] GameObject leftBoundObj;
    [SerializeField] GameObject rightBoundObj;
    [Range(0.01f, 1)][SerializeField] float spawnAreaCoverage;

    Vector3 leftBound;
    Vector3 rightBound;

    Vector3 currPoint;

    float leftBoundX;
    float rightBoundX;


    float totalDist;

    float distPerSpawn;

    //Attack Tracking
    int currWavesSpawned;

    int baseNumToSpawn;
    int baseSpawnReps;

    float waitTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        baseNumToSpawn = numToSpawn;
        baseSpawnReps = spawnReps;


        leftBoundObj = GameObject.FindWithTag("leftSpawnBound");
        rightBoundObj = GameObject.FindWithTag("rightSpawnBound");


        leftBound = leftBoundObj.transform.position;
        leftBoundX = leftBound.x;

        rightBound = rightBoundObj.transform.position;
        rightBoundX = rightBound.x;

        totalDist = Mathf.Abs(leftBoundX - rightBoundX);

        ResetValues();
        ResetSpawnLocation();
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfAttacking();
        if(isActive)
        {
            waitTime += Time.deltaTime;
            if(waitTime >= timeBetweenReps)
            {
                SummonMinions();
            }
        }
    }

    void SummonMinions()
    {
        for(int i = 0; i < numToSpawn; i++)
        {
            GameObject myMinion = Instantiate(enemyToSpawn, transform.position, transform.rotation);
            //GameObject myMinion = Instantiate(enemyToSpawn, currPoint, transform.rotation);
            //Do minion targeting stuff

            myMinion.GetComponent<DashToPoint>().CalculateBaseMoveSpeed();
            myMinion.GetComponent<DashToPoint>().SetPoint(currPoint);
            myMinion.GetComponent<DashToPoint>().TargetPoint();
            myMinion.GetComponent<DashToPoint>().SetFinalDirection(gameObject.transform.rotation);
            //
            //

            currPoint.x += distPerSpawn;
           
        }
        waitTime = 0;

        currWavesSpawned += 1;
        ResetSpawnLocation();
        IsAttackDone();

    }

    void CheckIfAttacking()
    {
        if (gameObject.GetComponent<BossPart>().attackingRightNow == true)
        {
            isActive = true;
        }
    }

    void IsAttackDone()
    {
        if (currWavesSpawned >= spawnReps)
        {
            EndAttack();
        }
    }

    void EndAttack()
    {
        gameObject.GetComponent<BossPart>().attackingRightNow = false;
        isActive = false;
        ResetValues();
        ResetSpawnLocation();
    }

    void ResetValues()
    {
        currWavesSpawned = 0;
        waitTime = 0;

        numToSpawn = baseNumToSpawn;
        spawnReps = baseSpawnReps;

        difficultyMod = gameObject.GetComponent<BossPart>().difficultyMod;

        switch (difficultyMod)
        {
            case 1:
                break;

            case 2:
                numToSpawn = (int)(numToSpawn * 1.5);
                spawnReps = (int)(spawnReps * 1.5);

                break;
            case 3:
                numToSpawn = (int)(numToSpawn * 2);
                spawnReps = (int)(spawnReps * 2);
                break;

            default:
                break;
        }

        
        distPerSpawn = (totalDist / numToSpawn) * spawnAreaCoverage;
    }

    void ResetSpawnLocation()
    {
        currPoint = new Vector3(leftBoundX, transform.position.y, transform.position.z + distInFrontTargetZ);
    }
}

