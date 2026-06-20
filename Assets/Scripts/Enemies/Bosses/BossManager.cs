using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class BossManager : MonoBehaviour
{
    public List<GameObject> bossParts = new List<GameObject>();
    public int difficultyModifer;
    [SerializeField] float timeBetweenAttacksBase;
    [SerializeField] float timeBetweenAttacksMod;
    [SerializeField] GameObject waveManager;
    [SerializeField] GameObject healingPad;
    [SerializeField] bool isFinalBoss;
    float waitTime;

    private void Awake()
    {
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (GameManager.instance != null && difficultyModifer == 0)
        {
            difficultyModifer = GameManager.instance.difficultyLevel;
            for(int i = 0; i < bossParts.Count; i++)
            {
                bossParts[i].GetComponent<BossPart>().difficultyMod = difficultyModifer;
                bossParts[i].GetComponent<BossPart>().UpdateHealth();
            }

        }
        if (difficultyModifer == 3)
        {
            isFinalBoss = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        waitTime += Time.deltaTime;
        if(waitTime >= (timeBetweenAttacksBase*timeBetweenAttacksMod))
        {
            TellPartToAttack();
        }
    }

    void TellPartToAttack()
    {
        int partToTell = Random.Range(0, bossParts.Count);

        bossParts[partToTell].GetComponent<BossPart>().AttackActivate();

        waitTime = 0;
    }

    public void ModifyAttackSpeed(float amount)
    {
        timeBetweenAttacksMod *= amount;
    }

    public void SetWaveManager(GameObject manager)
    {
        waveManager = manager;
    }

    public void GetManagerData()
    {
        difficultyModifer = waveManager.GetComponent<WaveManager>().difficultyMod;
    }

    void TellManagerBossDied()
    {
        if (waveManager != null)
        {
            waveManager.GetComponent<WaveManager>().OnBossDeath();
        }
    }

    private void OnDestroy()
    {
        TellManagerBossDied();
        if (healingPad != null && SceneManager.GetActiveScene().isLoaded != false)
        {
            Vector3 newPos = transform.position;
            newPos.z -= 10;
            Instantiate(healingPad, newPos, healingPad.transform.rotation);
        }
        if(isFinalBoss)
        {
            if (GameManager.instance != null)
            {
                GameManager.instance.GameWon();
            }
        }
    }

}
