using NUnit.Framework;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class EnemyFiring : MonoBehaviour
{
    [SerializeField] List<GunStats> gunList = new List<GunStats>();

    [SerializeField] Transform shootPosition;

    [SerializeField] AudioClip[] shootSounds;

    float shootTimer;

    int gunListPosition;
    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gunListPosition = 0;
    }

    // Update is called once per frame
    void Update()
    {
        shootTimer += Time.deltaTime;

        if (shootTimer >= gunList[gunListPosition].shootRate)
        {
            Shoot();
        }
    }
    void Shoot()
    {
        shootTimer = 0;
        for (int i = 0; i < gunList[gunListPosition].bulletsPerShot; i++)
        {
            GameObject myBullet = Instantiate(gunList[gunListPosition].bullet, shootPosition.position, transform.rotation);

            myBullet.GetComponent<damage>().damageAmount = gunList[gunListPosition].shootDamage;
            myBullet.GetComponent<damage>().bulletSpeed = Random.Range(gunList[gunListPosition].bulletSpeedMin, gunList[gunListPosition].bulletSpeedMax);
            myBullet.GetComponent<damage>().bulletDestroyTime = gunList[gunListPosition].bulletLifetime;
           

            myBullet.transform.Rotate(Random.Range(-(gunList[gunListPosition].spreadVert), gunList[gunListPosition].spreadVert), Random.Range(-(gunList[gunListPosition].spreadHoriz), gunList[gunListPosition].spreadHoriz), 0);
        }
        if (shootSounds.Length > 0)
        {
            AudioSource.PlayClipAtPoint(shootSounds[0], transform.position);
        }
    }

    public void GetGunStats(GunStats gunFound)
    {
        gunList.Add(gunFound);
        gunListPosition = gunList.Count - 1;



    }


}
