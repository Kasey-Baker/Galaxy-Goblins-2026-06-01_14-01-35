using UnityEngine;
using UnityEngine.InputSystem;

public class PlanetSizeScaler : MonoBehaviour
{

    [SerializeField] float planetSizeMin;
    [SerializeField] float planetSizeMax;

    [SerializeField] float timeToReachMaxSize;
    [SerializeField]
    public bool growthActive;

    float growthSpeed;
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetGrowthScaling();
    }

    // Update is called once per frame
    void Update()
    {
        if(growthActive && transform.localScale.x < planetSizeMax)
        {
            GrowPlanet();
        }
    }

    void GrowPlanet()
    {
        transform.localScale += new Vector3(growthSpeed, growthSpeed, growthSpeed) * Time.deltaTime;
    }

    public void SetGrowthScaling()
    {
        transform.localScale.Set(planetSizeMin, planetSizeMin, planetSizeMin);

        growthSpeed = (planetSizeMax - planetSizeMin) / timeToReachMaxSize;
    }

    public void SetGrowthTime(float newGrowthTime)
    {
        timeToReachMaxSize = newGrowthTime;
    }
}
