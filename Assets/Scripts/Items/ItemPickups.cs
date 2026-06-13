using UnityEditor.Build;
using UnityEngine;

public class ItemPickups : MonoBehaviour
{
   

    [SerializeField] private ItemData itemData;
    private GameObject Model;
    private bool isCollected;

    private void Start()
    {
        InitializeItem();
    }

    public void InitializeItem()
    {
        if (itemData == null) return;
        if (Model != null) Destroy(Model);
        if(itemData.modelPrefab != null)
        {
            Model = Instantiate(itemData.modelPrefab, transform.position, transform.rotation, transform);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (isCollected) return;
        if (other.CompareTag("Player"))
        {
            isCollected = true;
            OnPickup(other.gameObject);
        }
    }

    protected virtual void OnPickup (GameObject player)
    {
        if (itemData != null)
        {
            Debug.Log($"{itemData.itemName} Picked Up");

            PlayerControls Player = player.GetComponent<PlayerControls>();
            LevelSystem PlayerGun = player.GetComponent<LevelSystem>();
            if (Player != null)
            {
                Player.ApplyEffects(itemData);
                PlayerGun.FetchVariables();
            }
            Destroy(gameObject);
        }
    }

}
