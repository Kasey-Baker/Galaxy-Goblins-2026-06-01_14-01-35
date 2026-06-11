using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour
{
    [SerializeField] LineRenderer laserLine;

    [SerializeField] GameObject hitEffect;
    [SerializeField] Transform laserStartPos;

    [SerializeField] int laserMaxDist;
    [SerializeField] int laserDamage;
    [SerializeField] float damageRate;

    public bool IsFiring;

    bool isDamaging = false;

    private void Start()
    {
        IsFiring = false;
    }

    void Update()
    {
        if (IsFiring)
        {
            CreateLaser();
        }
        else
        {
            laserLine.SetPosition(0, laserStartPos.position);
            laserLine.SetPosition(1, laserStartPos.position);
            hitEffect.SetActive(false);
        }
    }

    void CreateLaser()
    {
        RaycastHit hit;
        if(Physics.Raycast(laserStartPos.position, laserStartPos.forward, out hit, laserMaxDist))
        {
            /*
            laserLine.SetPosition(0, laserStartPos.position);
            laserLine.SetPosition(1, hit.point);
            */
            hitEffect.SetActive(true);
            hitEffect.transform.position = laserStartPos.position;
            
            IDamage dmg = hit.collider.GetComponent<IDamage>();
            if (dmg != null && !isDamaging)
            {
                StartCoroutine(DamageTime(dmg));
            }

        }
            laserLine.SetPosition(0, laserStartPos.position);
            laserLine.SetPosition(1, laserStartPos.position + laserStartPos.forward * laserMaxDist);
            //hitEffect.SetActive(false);
    }

    IEnumerator DamageTime(IDamage d)
    {
        isDamaging = true;
        d.takeDamage(laserDamage);
        yield return new WaitForSeconds(damageRate);
        isDamaging = false; 
    }

}
