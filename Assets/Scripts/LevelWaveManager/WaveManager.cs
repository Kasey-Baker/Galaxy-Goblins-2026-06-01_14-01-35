using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] int numOfLargeSequences;
    [SerializeField] int numOfWavesPerSequence;
    [SerializeField] float timeBetweenWaves;

    [SerializeField] GameObject[] waveOptions;

    [SerializeField] GameObject bossToSpawn;

    [SerializeField] GameObject waveSpawnSpot; //Where to spawn the wave objects from


    [SerializeField] bool spawningActive;

    [SerializeField] bool bossSpawned;

    [SerializeField] bool itemsPresented;
    public bool itemChosen;

    int currWavesSpawned;

    int totalSectionsCleared;

    float waitTime;

    int waveRangeMin;
    [SerializeField] int waveRangeMax;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        waveSpawnSpot = GameObject.FindWithTag("waveSpawnSpot");
        waveRangeMin = 0;
        waveRangeMax = waveOptions.Length;

        itemsPresented = false;
        spawningActive = false;
        itemChosen = false;
        bossSpawned = false;
    }

    // Update is called once per frame
    void Update()
    {
        waitTime += Time.deltaTime;
        if(spawningActive && waitTime >= timeBetweenWaves && totalSectionsCleared < numOfLargeSequences)
        {
            SpawnWave();
        }
        else
        {
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

    void CheckSectionOver()
    {
        if(currWavesSpawned >= numOfWavesPerSequence)
        {
            totalSectionsCleared += 1;
            spawningActive = false;

            CreateItems();
        }
    }

    void CheckNextSection()
    {
        if(Input.GetButton("NextWave") && spawningActive == false)
        {
            if (totalSectionsCleared >= numOfLargeSequences && bossSpawned == false)
            {
                Instantiate(bossToSpawn);
                bossSpawned = true;
            }
            else
            {
                spawningActive = true;
                currWavesSpawned = 0;
            }
        }
    }

    void CreateItems()
    {
        //Should create random items for the player to pick from after the end of a section. This is dependent on the item functionality and the game manager so cannot yet be completed
    }

    

}
