using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using System.Linq;

//HOW TO USE:
/*
 * set currHealth serialize field to the HP value you want the enemy to have. This script should be added on to enemies.
 * 
 */
public class EnemyTakeDamage : MonoBehaviour, IDamage
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [Header("----- Setup -----")]
    [SerializeField] float currHealth;
    [SerializeField] Renderer rend;
    [SerializeField] float pointsOnDeath;
   

    [Header("----- Spawn Effects -----")]
    [SerializeField] GameObject spawnObj;
    [SerializeField] int numToSpawnStart;

    [Header("----- Hit Effects -----")]
    [SerializeField] GameObject spawnedObj;
    [SerializeField] int numToSpawnHit;
    [SerializeField] int numToSpawnDeath;
    [SerializeField] AudioSource myAudio;
    [SerializeField] AudioClip[] soundsOnHit;

    [Header("----- Death Behavior -----")]
    [SerializeField] GameObject deathSoundMaker;
    [SerializeField] AudioClip[] deathSounds;

    Color colorOrig;

    Renderer[] allRenders;
    Color[] allColors;

    float maxHealth;

    void Start()
    {

        maxHealth = currHealth;
        MakeGuts(numToSpawnStart, spawnObj);
        colorOrig = rend.material.color;
        GameManager.instance.updateEnemyCount(1);
        allRenders = GetComponentsInChildren<Renderer>();
        allColors = new Color[allRenders.Length];
        for (int i = 0; i < allRenders.Length; i++)
            {
            if (allRenders[i].material.HasProperty("_Color"))
            {
                allColors[i] = (allRenders[i].material.color);
            }
            }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetHealth(float newHP)
    {
        currHealth = newHP;
    }

    public float GetHealth()
    {
        return currHealth;
    }
    
    public void takeDamage(float amount)
    {

        
        currHealth -= amount;
        MakeGuts(numToSpawnHit);
        if (soundsOnHit.Length > 0 && currHealth > 0)
        {
            SetupHitSound();
        }
        if(currHealth <= 0)
        {
            GameManager.instance.AddPoints(pointsOnDeath);
            MakeGuts(numToSpawnDeath);
            Destroy(gameObject);
            if (deathSounds.Length > 0)
            {
                SetupDeathSound();
            }

        }
        else
        {
            StartCoroutine(flashRed());
        }
    }

    IEnumerator flashRed()
    {
        if (rend != null)
        {
            rend.material.color = Color.red;
        }
        for(int i = 0; i < allRenders.Length; i++)
        {
            if (allRenders[i] != null)
            {
                allRenders[i].material.color = Color.red;
            }
        }
        yield return new WaitForSeconds(0.1f);
        rend.material.color = colorOrig;
        for(int i = 0; i < allRenders.Length; i++)
        {
            if (allRenders[i] != null)
            {
                allRenders[i].material.color = allColors[i];
                Color newColor = allColors[i];
                //newColor.r = newColor.r * (currHealth / maxHealth);
                newColor.g = newColor.g * (currHealth / maxHealth);
                newColor.b = newColor.b * (currHealth / maxHealth);
                allRenders[i].material.color = newColor;
            }
        }
        
    }

    private void OnDestroy()
    {
        //When the game manager is set up, add pointsOnDeath to gamemanager points value
        GameManager.instance.updateEnemyCount(-1);

    }
    public void MakeGuts(int amount)
    {
        if (spawnedObj != null)
        {
            for (int i = 0; i < amount; i++)
            {
                Instantiate(spawnedObj, transform.position, transform.rotation);
            }
        }
        
    }

    public void MakeGuts(int amount, GameObject gutball)
    {
        for (int i = 0; i < amount; i++)
        {
            Instantiate(gutball, transform.position, transform.rotation);
        }

    }

    void SetupDeathSound()
    {
        GameObject myNoise = Instantiate(deathSoundMaker, transform.position, Quaternion.identity);
        myNoise.GetComponent<PlayDeathSound>().SetSound(deathSounds[Random.Range(0, deathSounds.Length)], myAudio.volume);
    }

    void SetupHitSound()
    {
        GameObject myNoise = Instantiate(deathSoundMaker, transform.position, Quaternion.identity);
        myNoise.GetComponent<PlayDeathSound>().SetSound(soundsOnHit[Random.Range(0, soundsOnHit.Length)], myAudio.volume);
    }
}
