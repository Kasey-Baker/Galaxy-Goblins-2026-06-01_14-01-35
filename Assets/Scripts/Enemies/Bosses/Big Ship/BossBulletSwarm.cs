using Unity.VisualScripting;
using UnityEngine;

public class BossBulletSwarm : MonoBehaviour
{
    [SerializeField] GameObject bulletToShoot;
    [SerializeField] int numToShoot;
    [SerializeField] int numRepetitions;
    [SerializeField] float angleOffsetPerRep;
    [SerializeField] float timeBetweenShots;
    [SerializeField] int difficultyMod;
    [SerializeField] AudioSource myAudio;
    [SerializeField] AudioClip[] mySounds;

    public bool isActive;

    int baseNumToShoot;
    int baseNumRepetitions;


    float currAngleOffset;
    float waitTime;
    float repsCompleted;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        baseNumToShoot = numToShoot;
        baseNumRepetitions = numRepetitions;
        ResetValues();
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfAttacking();
        if (isActive)
        {
            waitTime += Time.deltaTime;
            if (waitTime >= timeBetweenShots)
            {
                CircleBlast();
            }
        }
    }

    void CircleBlast()
    {
        float angleModPerBullet = 360 / numToShoot;
        float angleModTotal = 0 + currAngleOffset;

        myAudio.PlayOneShot(mySounds[Random.Range(0, mySounds.Length)]);

        for (int i = 0; i < numToShoot; i++)
        {
            GameObject myBullet = Instantiate(bulletToShoot, transform.position, transform.rotation);
            myBullet.transform.Rotate(0f, angleModTotal, 0f);
            angleModTotal += angleModPerBullet;
        }
        currAngleOffset += angleOffsetPerRep;
        waitTime = 0;
        repsCompleted += 1;

        IsAttackDone();
      
    }

    void CheckIfAttacking()
    {
        if(gameObject.GetComponent<BossPart>().attackingRightNow == true)
        {
            isActive = true;
        }    
    }

    void IsAttackDone()
    {
        if(repsCompleted >= numRepetitions)
        {
            EndAttack();
        }
    }

    void EndAttack()
    {
        gameObject.GetComponent<BossPart>().attackingRightNow = false;
        isActive = false;
        ResetValues();
    }

    void ResetValues()
    {
        currAngleOffset = 0;
        repsCompleted = 0;
        waitTime = 0;

        numRepetitions = baseNumRepetitions;
        numToShoot = baseNumToShoot;

        difficultyMod = gameObject.GetComponent<BossPart>().difficultyMod;

        switch (difficultyMod)
        {
            case 1:
                break;

            case 2:
                numToShoot = (int)(numToShoot * 1.5);
                numRepetitions = (int)(numRepetitions * 1.5);

                break;
            case 3:
                numToShoot = (int)(numToShoot * 2);
                numRepetitions = (int)(numRepetitions * 2);
                break;

            default:
                break;
        }
    }
}
