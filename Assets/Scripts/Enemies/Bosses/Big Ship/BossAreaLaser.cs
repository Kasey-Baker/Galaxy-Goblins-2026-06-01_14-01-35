using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AreaLaser : MonoBehaviour
{
    [SerializeField] GameObject myLaser;
    [SerializeField] int difficultyMod;
    [SerializeField] AudioSource myAudio;
    [SerializeField] AudioClip[] mySounds;
    [SerializeField] AudioClip laserFire;
    [SerializeField] float laserAttackLength;
    [SerializeField] float laserSpeed;
    [SerializeField] Vector3 pointDir;
    [SerializeField] GameObject player;

    float currAttackDuration;

    float baseLaserAttackLength;
    float baseLaserSpeed;

    public bool isActive;

    bool soundPlaying;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        baseLaserAttackLength = laserAttackLength;
        baseLaserSpeed = laserSpeed;
        ResetValues();
        soundPlaying = false;

        player = GameManager.instance.player;
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfAttacking();
        if (isActive)
        {
            currAttackDuration += Time.deltaTime;
            LaserBeam();
        }
    }

    void LaserBeam()
    {
        if(!soundPlaying)
        {
            myAudio.PlayOneShot(laserFire);
        }
        if (currAttackDuration < laserAttackLength)
        {
            FaceTarget();
        }
        else
        {
            IsAttackDone();
        }
    }

    void FaceTarget()
    {
        myLaser.GetComponent<Laser>().IsFiring = true;
        pointDir = player.transform.position - gameObject.transform.position;
        pointDir.y = 0;

        

        Vector3 newDir = Vector3.RotateTowards(transform.forward, pointDir, Time.deltaTime * laserSpeed, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDir);
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
        if (currAttackDuration >= laserAttackLength)
        {
            EndAttack();
        }
    }

    void EndAttack()
    {
        gameObject.GetComponent<BossPart>().attackingRightNow = false;
        myLaser.GetComponent<Laser>().IsFiring = false;
        isActive = false;
        ResetValues();
        myAudio.Stop();
    }

    void ResetValues()
    {
        currAttackDuration = 0;
        laserAttackLength = baseLaserAttackLength;
        laserSpeed = baseLaserSpeed;


        difficultyMod = gameObject.GetComponent<BossPart>().difficultyMod;

        switch (difficultyMod)
        {
            case 1:
                break;

            case 2:
                laserAttackLength = (float)(laserAttackLength * 1.5);
                laserSpeed = (float)(laserSpeed * 1.5);

                break;
            case 3:
                laserAttackLength = (float)(laserAttackLength * 2);
                laserSpeed = (float)(laserSpeed * 2);
                break;

            default:
                break;
        }

        int lessPartsMod = gameObject.GetComponent<BossPart>().GetOwner().GetComponent<BossManager>().bossParts.Count;

        laserSpeed *= 1 + 0.35f*(4 - lessPartsMod);



    }
}
