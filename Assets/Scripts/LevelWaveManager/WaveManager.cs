using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveManager : MonoBehaviour
{
    [SerializeField] int numOfLargeSequences;
    [SerializeField] int numOfWavesPerSequence;
    [SerializeField] float timeBetweenWaves;
    public int difficultyMod;

    [SerializeField] GameObject[] waveOptions; //Use this one for spawning, the rest are pre-sets to choose from
    [SerializeField] GameObject[] waveOptionsTutorial;
    [SerializeField] GameObject[] waveOptionsEasy;
    [SerializeField] GameObject[] waveOptionsNormal;
    [SerializeField] GameObject[] waveOptionsHard;

    [SerializeField] GameObject[] bossToSpawnOptions;
    [SerializeField] GameObject bossToSpawn;
    [SerializeField] GameObject bossSpawnSpot;

    [SerializeField] GameObject waveSpawnSpot; //Where to spawn the wave objects from


    [SerializeField] bool spawningActive;
    [SerializeField] GameObject myPlanet;
    [SerializeField] bool bossSpawned;
    [SerializeField] bool bossDefeated;

    [SerializeField] GameObject[] portalSpots;
    [SerializeField] GameObject[] portals;
    [SerializeField] bool portalsCreated;


    [SerializeField] GameObject[] itemSpots;
    [SerializeField] GameObject[] itemList;
    [SerializeField] bool itemsPresented;
    public bool itemChosen;

    int currWavesSpawned;

    int totalSectionsCleared;

    float waitTime;

    int waveRangeMin;
    [SerializeField] int waveRangeMax;

    [SerializeField] GameObject helpfulText;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(GameManager.instance != null)
        {
            //Switches and sets the types of waves to spawn based on current difficulty.
            //If number of waves to spawn is currently unset, it also sets it.
            difficultyMod = GameManager.instance.difficultyLevel;
            switch(GameManager.instance.difficultyLevel)
            {
                case 1:

                    waveOptions = waveOptionsEasy;
                    if (numOfLargeSequences == 0)
                    {
                        numOfLargeSequences = 5;
                        numOfWavesPerSequence = 5;
                        timeBetweenWaves = 5;
                    }

                    break;

                case 2:

                    waveOptions = waveOptionsNormal;
                    if (numOfLargeSequences == 0)
                    {
                        numOfLargeSequences = 8;
                        numOfWavesPerSequence = 6;
                        timeBetweenWaves = 4;
                    }

                    break;

                case 3:

                    waveOptions = waveOptionsHard;
                    if (numOfLargeSequences == 0)
                    {
                        numOfLargeSequences = 10;
                        numOfWavesPerSequence = 7;
                        timeBetweenWaves = 3;
                    }

                    break;

                default:

                    waveOptions = waveOptionsTutorial;
                    if (numOfLargeSequences == 0)
                    {
                        numOfLargeSequences = 3;
                        numOfWavesPerSequence = 4;
                        timeBetweenWaves = 6;
                    }

                    break;
            }
        }
        waveSpawnSpot = GameObject.FindWithTag("waveSpawnSpot");
        waveRangeMin = 0;
        waveRangeMax = waveOptions.Length;

        if (bossToSpawn == null)
        {
            switch (SceneManager.GetActiveScene().name)
            {
                case "Grass Level":

                    bossToSpawn = bossToSpawnOptions[0];

                    break;

                case "Water Level":

                    bossToSpawn = bossToSpawnOptions[1];

                    break;

                case "Volcano Level":

                    bossToSpawn = bossToSpawnOptions[2];

                    break;

                default:

                    bossToSpawn = null;

                    break;

            }
        }

        if(myPlanet != null)
        {
            float myTime = numOfLargeSequences * numOfWavesPerSequence * timeBetweenWaves;
            myPlanet.GetComponent<PlanetSizeScaler>().SetGrowthTime(myTime);
            myPlanet.GetComponent<PlanetSizeScaler>().SetGrowthScaling();
        }

        itemsPresented = false;
        spawningActive = false;
        itemChosen = false;
        itemsPresented = false;
        bossSpawned = false;
        portalsCreated = false;
    }

    // Update is called once per frame
    void Update()
    {
        waitTime += Time.deltaTime;
        if(spawningActive && waitTime >= timeBetweenWaves && totalSectionsCleared < numOfLargeSequences)
        {
            SpawnWave();
            if (myPlanet != null)
            {
                myPlanet.GetComponent<PlanetSizeScaler>().growthActive = true;
            }
        }
        else
        {
            AreEnemiesGone();
            CheckNextSection();
        }



    }

    void SpawnWave()
    {
        Instantiate(waveOptions[Random.Range(waveRangeMin, waveRangeMax)], waveSpawnSpot.transform.position, transform.rotation);
        currWavesSpawned += 1;
        waitTime = 0;

        CheckSectionOver();
    }

    void AreEnemiesGone()
    {
        if(GameManager.instance.getEnemyCount() <= 0 && itemsPresented == false && spawningActive == false)
        {
            CreateItems();
            itemsPresented = true;
            helpfulText.SetActive(true);
            if (myPlanet != null)
            {
                myPlanet.GetComponent<PlanetSizeScaler>().growthActive = false;
            }
        }
    }

    void CheckSectionOver()
    {
        if(currWavesSpawned >= numOfWavesPerSequence)
        {
            totalSectionsCleared += 1;
            spawningActive = false;

         
        }
    }

    void CheckNextSection()
    {
        if (Input.GetButton("NextWave") && spawningActive == false && portalsCreated == false)
        {
            helpfulText.SetActive(false);
            itemsPresented = false;
            if (totalSectionsCleared >= numOfLargeSequences && bossSpawned == false)
            {
                if (bossToSpawn != null)
                {
                    SummonBoss();
                }
                else if (portalsCreated == false)
                {
                    CreatePortals();
                }
            }
            else
            {
                spawningActive = true;
                currWavesSpawned = 0;
            }
        }
    }

    void SummonBoss()
    {
        GameObject myBoss = Instantiate(bossToSpawn, bossSpawnSpot.transform.position, Quaternion.identity);
        myBoss.GetComponent<BossManager>().SetWaveManager(gameObject);
        bossSpawned = true;
    }

    void CreateItems()
    {
        //Should create random items for the player to pick from after the end of a section. This is dependent on the item functionality and the game manager so cannot yet be completed
        for(int i = 0; i < itemSpots.Length; i++)
        {
            Instantiate(itemList[Random.Range(0, itemList.Length)], itemSpots[i].transform.position, Quaternion.identity);
        }
    }

    void CreatePortals()
    {
        for(int i = 0; i < portals.Length; i++)
        {
            Instantiate(portals[i], portalSpots[i].transform.position, Quaternion.identity);
        }
        portalsCreated = true;
    }
    
    public void OnBossDeath()
    {
        bossDefeated = true;
        CreatePortals();
    }

}
