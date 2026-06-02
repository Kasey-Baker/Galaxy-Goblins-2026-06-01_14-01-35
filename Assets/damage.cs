using UnityEngine;
using System.Collections;
public class damage : MonoBehaviour
{
    enum damageType { bullet, stationsary, DOT }
    [SerializeField] damageType type;
    [SerializeField] Rigidbody rb;

    public float damageAmount;
    [SerializeField] float damageRate;
    public float bulletSpeed;
    [SerializeField] int bulletDestroyTime;
    [SerializeField] ParticleSystem hitEffect;
    [SerializeField] AudioClip playerHitSound;
    [SerializeField] AudioClip otherHitSound;
    [SerializeField] GameObject ignoredObject;
    [SerializeField] string tagToIgnore;
    bool isDamaging;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (type == damageType.bullet )
        {
            rb.linearVelocity = transform.forward * bulletSpeed;

            Destroy(gameObject, bulletDestroyTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }
        IDamage dmg = other.GetComponent<IDamage>();
        if (other.gameObject.tag != tagToIgnore)
        {


            if (dmg != null && type != damageType.DOT)
            {
                if (playerHitSound != null)
                {
                    AudioSource.PlayClipAtPoint(playerHitSound, transform.position);
                }
                dmg.takeDamage(damageAmount);
            }
            else
            {
                if (otherHitSound != null)
                {
                    AudioSource.PlayClipAtPoint(otherHitSound, transform.position);
                }
            }

            if (type == damageType.bullet)
            {
                if (hitEffect != null)
                {
                    Instantiate(hitEffect, transform.position, Quaternion.identity);
                }

                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.isTrigger) 
            return;

        IDamage dmg = other.GetComponent<IDamage>();
        if(dmg != null && type == damageType.DOT && !isDamaging)
        {
            StartCoroutine(damageOther(dmg));
        }
    }

    IEnumerator damageOther(IDamage d)
    {
        isDamaging = true;
        d.takeDamage(damageAmount);
        yield return new WaitForSeconds(damageRate);
        isDamaging = false;
    }
}
