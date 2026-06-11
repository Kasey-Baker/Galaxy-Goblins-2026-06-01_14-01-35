using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    public enum ShotPattern
    {
        SingleForward, DualSideBySide, ThreeShotCone
    }

    [Header("Level Progress")]
    public int currentScore = 0;
    public int currentLevel = 1;
    public int[] scoreThresholds = { 1000, 2500, 5000 };

    [Header("Player Stats")]
    public int bulletDamage = 10;
    public int bulletAmount = 1;
    public ShotPattern currentPattern = ShotPattern.SingleForward;

    [Header("Pattern Adjustments")]
    [SerializeField] float dualShotOffset = 0.3f;
    [SerializeField] float coneAngle = 15f;

    
    private PlayerControls playerControls;
    private float fireWait;
    private float fireRate;
    private GameObject bulletPrefab;

    void Start()
    {

        playerControls = GetComponent<PlayerControls>();

        if (playerControls != null)
        {
            FetchVariables();
        }
    }

    void Update()
    {

        fireWait += Time.deltaTime;


        if (Input.GetButton("Fire1") && fireWait >= fireRate && bulletPrefab != null)
        {
            HandlePatternShooting();
        }
    }

    private void HandlePatternShooting()
    {
        fireWait = 0f; 

        switch (currentPattern)
        {
            case ShotPattern.SingleForward:
                SpawnAndConfigureBullet(transform.position, 0f);
                break;

            case ShotPattern.DualSideBySide:
                Vector3 leftPos = transform.position - (transform.right * dualShotOffset);
                Vector3 rightPos = transform.position + (transform.right * dualShotOffset);

                SpawnAndConfigureBullet(leftPos, 0f);
                SpawnAndConfigureBullet(rightPos, 0f);
                break;

            case ShotPattern.ThreeShotCone:
                SpawnAndConfigureBullet(transform.position, 0f);         
                SpawnAndConfigureBullet(transform.position, -coneAngle); 
                SpawnAndConfigureBullet(transform.position, coneAngle);  
                break;
        }
    }

    private void SpawnAndConfigureBullet(Vector3 position, float angleY)
    {

        GameObject newBullet = Instantiate(bulletPrefab, position, Quaternion.Euler(0f, transform.eulerAngles.y + angleY, 0f));

        
        damage bulletDamageScript = newBullet.GetComponent<damage>();
        if (bulletDamageScript != null)
        {
            
            bulletDamageScript.damageAmount = bulletDamage;
        }
    }

    public void AddScore(int points)
    {
        currentScore += points;
        CheckForLevelUp();
    }

    private void CheckForLevelUp()
    {
        if (currentLevel - 1 >= scoreThresholds.Length) return;

        int requiredScore = scoreThresholds[currentLevel - 1];

        if (currentScore >= requiredScore)
        {
            LevelUp();
            CheckForLevelUp();
        }
    }

    private void LevelUp()
    {
        currentLevel++;
        bulletDamage += 5;
        bulletAmount += 1;

        UpdateShotPattern();
    }

    private void UpdateShotPattern()
    {
        if (currentLevel == 1) currentPattern = ShotPattern.SingleForward;
        else if (currentLevel == 2) currentPattern = ShotPattern.DualSideBySide;
        else currentPattern = ShotPattern.ThreeShotCone;
    }

    private void FetchVariables()
    {
        // Fetches other variables from other scripts.
        System.Reflection.FieldInfo rateField = typeof(PlayerControls).GetField("firerate", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        System.Reflection.FieldInfo listField = typeof(PlayerControls).GetField("bulletList", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        System.Reflection.FieldInfo currField = typeof(PlayerControls).GetField("currBullet", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        if (rateField != null) fireRate = (float)rateField.GetValue(playerControls);

        if (listField != null && currField != null)
        {
            GameObject[] list = (GameObject[])listField.GetValue(playerControls);
            int index = (int)currField.GetValue(playerControls);
            if (list != null && list.Length > index)
            {
                bulletPrefab = list[index];
            }


        }
    }
}
