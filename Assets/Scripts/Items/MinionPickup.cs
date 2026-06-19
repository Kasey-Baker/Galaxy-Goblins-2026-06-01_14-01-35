using UnityEngine;

public class MinionPickup : ItemPickups
{
    [SerializeField] private GameObject minionPrefab;

    protected override void OnPickup(GameObject player)
    {
        if (minionPrefab != null)
        {
            Instantiate(minionPrefab, player.transform.position, Quaternion.identity);
        }

        base.OnPickup(player);
    }
}
