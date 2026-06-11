using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    public int currentScore = 0;
    public int currentLevel = 1;
    public int[] scoreThresholds = { 1000, 2500, 5000 };

    public int bulletdamage = 10;
    public int bulletAmount = 1;

    public void AddScore(int points)
    {
        currentScore += points;


    }
    private void CheckForLevelUp()
    {
        if (currentLevel <= scoreThresholds.Length)
        {
            int requierdScore=scoreThresholds[currentLevel-1];

            if (currentScore >= requierdScore)
            {
                LevelUp();
            }
           


        }

    }

    private void LevelUp()
    {
        currentLevel++;
        bulletdamage += 5;
        bulletAmount += 1;

    }

}
