using UnityEngine;

public class ItemPickups : MonoBehaviour
{
   

    [SerializeField] private ItemData itemData;
    private GameObject Model;

    private void Start()
    {
        InitializeItem();
    }

    public void InitializeItem()
    {
        if (itemData == null) return;
        if (Model == null) Destroy(Model);
        if(itemData.modelPrefab != null)
        {
            Model = Instantiate(itemData.modelPrefab, transform.position, transform.rotation, transform);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnPickup(other.gameObject);
        }
    }

    protected virtual void OnPickup (GameObject player)
    {
        Debug.Log($"{itemData.itemName} Picked Up");

        Destroy(gameObject);
    }

}
