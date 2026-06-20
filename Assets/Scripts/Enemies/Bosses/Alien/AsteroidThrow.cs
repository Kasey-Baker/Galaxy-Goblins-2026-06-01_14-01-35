using NUnit.Framework;
using System.Data;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using System.Collections.Generic;

public class AsteroidThrow : MonoBehaviour
{

    [Header("Attack Setup")]
    [SerializeField] CharacterController controller;
    [SerializeField] GameObject verticalAttackPoint;
    [SerializeField] GameObject[] spawnPoints;
    [SerializeField] float degreesToRotate;
    [SerializeField] float rotateSpeed;
    [SerializeField] float moveSpeed;
    [SerializeField] int difficultyMod;

    [Header("Asteroid Stats")]
    [SerializeField] GameObject asteroidToSpawn;
    [SerializeField] float asteroidSize;
    [SerializeField] float asteroidHealth;
    [SerializeField] float asteroidSpeed;
    [SerializeField] int numRepetitions;
    [SerializeField] float timeBetweenRepetitions;
    [SerializeField] List<GameObject> asteroidList = new List<GameObject>();


    [Header("Sounds")]
    [SerializeField] AudioSource myAudio;
    [SerializeField] AudioClip[] mySounds;

    Vector3 myInitDirection;
    Vector3 homePoint;

    public bool isActive;
    [SerializeField] AttackState currState;

    float baseAsteroidSize;
    float baseAsteroidHealth;
    float baseAsteroidSpeed;
    int baseNumRepetitions;
    float baseTimeBetweenRepetitions;
  

    float waitTime;
    int repsCompleted;
    float degreesRotated;
    enum AttackState {ReachingPointVert = 0, GrabbingAsteroids = 1, CreatingAsteroids = 2, ThrowingAsteroids = 3, AtRest = 4, ReachingPointHoriz = 5, ReturningPointHoriz = 6, ReturningPointVert = 7}



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        homePoint = transform.position;
        currState = AttackState.AtRest;
        baseAsteroidSpeed = asteroidSpeed;
        baseAsteroidSize = asteroidSize;
        baseAsteroidHealth = asteroidHealth;
        baseNumRepetitions = numRepetitions;
        baseTimeBetweenRepetitions = timeBetweenRepetitions;

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
                case AttackState.ReachingPointVert:

                    TravelToPointVert();
                    IsCloseToPointVert();

                    break;

                case AttackState.ReachingPointHoriz:

                    TravelToPointHoriz();
                    IsCloseToPointHoriz();

                    break;

                case AttackState.GrabbingAsteroids:

                    waitTime += Time.deltaTime;
                    if (waitTime > timeBetweenRepetitions)
                    {
                        GrabAsteroids();
                    }

                    break;

                case AttackState.CreatingAsteroids:

                    CreateAsteroids();
                    if (mySounds.Length > 0)
                    {
                        myAudio.PlayOneShot(mySounds[Random.Range(0, mySounds.Length)]);
                    }

                    break;

                case AttackState.ThrowingAsteroids:

                    ThrowAsteroids();

                    break;

                case AttackState.ReturningPointHoriz:

                    TravelToHomeHoriz();
                    IsCloseToHomeHoriz();

                    break;

                case AttackState.ReturningPointVert:

                    TravelToHomeVert();
                    IsCloseToHomeVert();

                    break;

                case AttackState.AtRest:

                    IsAttackDone();
                    if (isActive)
                    {
                        BeginAttack();
                    }
                    else
                    {
                        EndAttack();
                    }

                        break;

                default:

                    break;
            }
        }
    }

    void SafetyCheck()
    {
        if (gameObject.GetComponent<TentacleAttack>().isActive)
        {
            isActive = false;
        }
    }

    void BeginAttack()
    {
        currState = AttackState.ReachingPointVert;
    }


    //Travelling to asteroid point
    #region
    void TravelToPointVert()
    {
        myInitDirection = verticalAttackPoint.transform.position - gameObject.transform.position;
        myInitDirection = new Vector3(0, 0, myInitDirection.z);
        Quaternion rot = Quaternion.LookRotation(new Vector3(0, 0, myInitDirection.z));
        controller.Move(myInitDirection.normalized * moveSpeed * Time.deltaTime);
    }

    
    void TravelToPointHoriz()
    {
        myInitDirection = verticalAttackPoint.transform.position - gameObject.transform.position;
        myInitDirection = new Vector3(myInitDirection.x, 0, 0);
        Quaternion rot = Quaternion.LookRotation(new Vector3(myInitDirection.x, 0, 0));
        controller.Move(myInitDirection.normalized * moveSpeed * Time.deltaTime);
    }

    void IsCloseToPointVert()
    {
        if (Mathf.Abs(verticalAttackPoint.transform.position.z - transform.position.z) <= 1)
        {
            currState = AttackState.ReachingPointHoriz;
            controller.enabled = false;
            transform.position = new Vector3(transform.position.x, transform.position.y, verticalAttackPoint.transform.position.z);
            controller.enabled = true;
        }
    }

    void IsCloseToPointHoriz()
    {
        if (Mathf.Abs(verticalAttackPoint.transform.position.x - transform.position.x) <= 1)
        {
            currState = AttackState.GrabbingAsteroids;
            controller.enabled = false;
            transform.position = new Vector3(verticalAttackPoint.transform.position.x, transform.position.y, transform.position.z);
            controller.enabled = true;
        }
    }

    void TravelToHomeVert()
    {
        myInitDirection = homePoint - gameObject.transform.position;
        myInitDirection = new Vector3(0, 0, myInitDirection.z);
        Quaternion rot = Quaternion.LookRotation(new Vector3(0, 0, myInitDirection.z));
        controller.Move(myInitDirection.normalized * moveSpeed * Time.deltaTime);
    }

    void TravelToHomeHoriz()
    {
        myInitDirection = homePoint - gameObject.transform.position;
        myInitDirection = new Vector3(myInitDirection.x, 0, 0);
        Quaternion rot = Quaternion.LookRotation(new Vector3(myInitDirection.x, 0, 0));
        controller.Move(myInitDirection.normalized * moveSpeed * Time.deltaTime);
    }

    void IsCloseToHomeVert()
    {
        if (Mathf.Abs(homePoint.z - transform.position.z) <= 1)
        {
            currState = AttackState.AtRest;
            controller.enabled = false;
            transform.position = new Vector3(transform.position.x, transform.position.y, homePoint.z);
            controller.enabled = true;
        }
    }

    void IsCloseToHomeHoriz()
    {
        if (Mathf.Abs(homePoint.x - transform.position.x) <= 1)
        {
            currState = AttackState.ReturningPointVert;
            controller.enabled = false;
            transform.position = new Vector3(homePoint.x, transform.position.y, transform.position.z);
            controller.enabled = true;
        }
    }
    #endregion

    void GrabAsteroids()
    {
        transform.Rotate(0f, rotateSpeed * Time.deltaTime, 0f);
        degreesRotated += rotateSpeed * Time.deltaTime;
        if(Mathf.Abs(degreesRotated) >= Mathf.Abs(degreesToRotate))
        {
            currState = AttackState.CreatingAsteroids;
            degreesRotated = 0;
        }
    }

    void ThrowAsteroids()
    {
        transform.Rotate(0f, -rotateSpeed * Time.deltaTime, 0f);
        degreesRotated += rotateSpeed * Time.deltaTime;
        for(int i = 0; i < spawnPoints.Length; i++)
        {
            asteroidList[i].transform.position = spawnPoints[i].transform.position;
            asteroidList[i].GetComponentInChildren<CharacterController>().enabled = false;
        }
        if (Mathf.Abs(degreesRotated) >= Mathf.Abs(degreesToRotate))
        {
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                asteroidList[i].GetComponentInChildren<CharacterController>().enabled = true;
            }
            repsCompleted += 1;
            if (repsCompleted >= numRepetitions)
            {
                currState = AttackState.ReturningPointHoriz;
            }
            else
            {
                waitTime = 0;
                degreesRotated = 0;
                currState = AttackState.GrabbingAsteroids;
            }
                asteroidList.Clear();
        }
    }

    void CreateAsteroids()
    {
        for(int i = 0; i < spawnPoints.Length; i++)
        {
            CreateAsteroid(spawnPoints[i].transform.position, i);
        }
        currState = AttackState.ThrowingAsteroids;
    }

    void CreateAsteroid(Vector3 location, int arrayPlace)
    {
        GameObject myAsteroid = Instantiate(asteroidToSpawn, location, Quaternion.identity);
        myAsteroid.transform.localScale = new Vector3(asteroidSize, asteroidSize, asteroidSize);
        myAsteroid.GetComponentInChildren<InanimateTakeDamage>().SetHealth(asteroidHealth);
        myAsteroid.GetComponent<EnemyDashingShip>().SetMoveSpeed(asteroidSpeed);
        asteroidList.Add(myAsteroid);

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
        degreesRotated = 0;
        repsCompleted = 0;
        waitTime = 0;
        currState = AttackState.AtRest;

        numRepetitions = baseNumRepetitions;
        asteroidHealth = baseAsteroidHealth;
        asteroidSize = baseAsteroidSize;
        asteroidSpeed = baseAsteroidSpeed;
        timeBetweenRepetitions = baseTimeBetweenRepetitions;

        difficultyMod = gameObject.GetComponent<BossPart>().difficultyMod;

        switch (difficultyMod)
        {
            case 1:
                break;

            case 2:
                timeBetweenRepetitions = (float)(timeBetweenRepetitions * 1.5);
                numRepetitions = (int)(numRepetitions * 1.5);
                asteroidHealth = (float)(asteroidHealth * 1.5);
                asteroidSpeed = (float)(asteroidSpeed * 1.5);
                asteroidSize = (float)(asteroidSize * 1.5);

                break;
            case 3:
                timeBetweenRepetitions = (float)(timeBetweenRepetitions * 2);
                numRepetitions = (int)(numRepetitions * 2);
                asteroidHealth = (float)(asteroidHealth * 2);
                asteroidSpeed = (float)(asteroidSpeed * 2);
                asteroidSize = (float)(asteroidSize * 2);
                break;

            default:
                break;
        }
    }
}
