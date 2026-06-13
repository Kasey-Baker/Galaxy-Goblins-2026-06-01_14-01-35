using UnityEngine;


public class WaveSpawner : MonoBehaviour
{
    enum SpawnPattern { SameSpotOverTime, HorizontalAllAtOnce, HorizontalOverTime}

    enum SpawnSide { Left, Right, CenterLeft, CenterRight}

    [Header ("Spawn Pattern Options")]
    [SerializeField] SpawnPattern spawnType;
    [SerializeField] SpawnSide spawnOnSide;
    [Range(0.01f, 1f)] [SerializeField] float spawnAreaCoverage;
    [SerializeField] float timeBetweenSpawns;


    [Header("Enemy Info")]
    [SerializeField] GameObject enemyToSpawn;
    [SerializeField] int spawnNumber;


    [Header("Spawn Area Bounding")]
    [SerializeField] LayerMask layerWithBounds;

    [SerializeField] GameObject leftBoundObj;
    [SerializeField] GameObject rightBoundObj;
    [SerializeField] bool randomizeLocation;

   
    Vector3 leftBound;
    Vector3 rightBound;

    Vector3 currPoint;

    float leftBoundX;
    float rightBoundX;

    float waitTime = 0;

    float totalDist;

    float distPerSpawn;

    int numSpawned;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {



        //Sets up spawning locations. Right now leftBoundObj and rightBoundObj are drag-ons, but should be integrated with gamemanager once gamemanager is set up.

        leftBoundObj = GameObject.FindWithTag("leftSpawnBound");
        rightBoundObj = GameObject.FindWithTag("rightSpawnBound");


        leftBound = leftBoundObj.transform.position;
        leftBoundX = leftBound.x;

        rightBound = rightBoundObj.transform.position;
        rightBoundX = rightBound.x;

        switch(spawnOnSide)
        {
            case SpawnSide.Left:

                totalDist = Mathf.Abs(leftBoundX - rightBoundX);
                currPoint = new Vector3(leftBoundX, transform.position.y, transform.position.z);
                distPerSpawn = (totalDist / spawnNumber)*spawnAreaCoverage;
                break;

            case SpawnSide.CenterRight:

                totalDist = Mathf.Abs(leftBoundX - rightBoundX);
                currPoint = new Vector3(leftBoundX+(totalDist/2), transform.position.y, transform.position.z);
                distPerSpawn = (totalDist / (2*spawnNumber))*spawnAreaCoverage;
                break;

            case SpawnSide.Right:

                totalDist = Mathf.Abs(leftBoundX - rightBoundX);
                currPoint = new Vector3(rightBoundX, transform.position.y, transform.position.z);
                distPerSpawn = (totalDist / spawnNumber)*-1*spawnAreaCoverage;
                break;

            case SpawnSide.CenterLeft:

                totalDist = Mathf.Abs(leftBoundX - rightBoundX);
                currPoint = new Vector3(rightBoundX-(totalDist / 2), transform.position.y, transform.position.z);
                distPerSpawn = (totalDist / (2*spawnNumber))*-1*spawnAreaCoverage;
                break;

            default:
                break;
        }
        

 
        if(randomizeLocation)
        {
            transform.position = new Vector3(Random.Range(leftBoundX, rightBoundX), transform.position.y, transform.position.z);
        }

    

        
    }

    // Update is called once per frame
    void Update()
    {
        waitTime += Time.deltaTime;

        switch (spawnType)
        {
            case SpawnPattern.SameSpotOverTime:

                if(waitTime >= timeBetweenSpawns)
                {
                    SpawnAtSelf();
                }

                break;

            case SpawnPattern.HorizontalAllAtOnce:

                SpawnHorizontalInstant();

                break;

            case SpawnPattern.HorizontalOverTime:

                if (waitTime >= timeBetweenSpawns)
                {
                    SpawnHorizontalOverTime();
                }

                break;

            default:
                break;
        }

    }

    void SpawnAtSelf()
    {
        Instantiate(enemyToSpawn, transform.position, transform.rotation);
        numSpawned += 1;
        waitTime = 0;

        DoneSpawnCheck();
    }

    void SpawnHorizontalInstant()
    {
   
        while(numSpawned < spawnNumber)
        {
            Instantiate(enemyToSpawn, currPoint, transform.rotation);
            currPoint.x += distPerSpawn;
            numSpawned += 1;
        }

        DoneSpawnCheck();
    }

    void SpawnHorizontalOverTime()
    {
        Instantiate(enemyToSpawn, currPoint, transform.rotation);
        currPoint.x += distPerSpawn;
        numSpawned += 1;
        waitTime = 0;

        DoneSpawnCheck();
    }

    void DoneSpawnCheck()
    {
        if(numSpawned >= spawnNumber)
        {
            Destroy(gameObject);
        }
    }

}
