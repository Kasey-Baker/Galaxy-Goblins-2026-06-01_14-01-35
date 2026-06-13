using UnityEngine;
using System.Collections.Generic;

public class BossManager : MonoBehaviour
{
    public List<GameObject> bossParts = new List<GameObject>();
    public int difficultyModifer;
    [SerializeField] float timeBetweenAttacksBase;
    [SerializeField] float timeBetweenAttacksMod;
    [SerializeField] GameObject waveManager;
    [SerializeField] bool isFinalBoss;
    float waitTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
        if(isFinalBoss)
        {
            GameManager.instance.GameWon();
        }
    }

}
