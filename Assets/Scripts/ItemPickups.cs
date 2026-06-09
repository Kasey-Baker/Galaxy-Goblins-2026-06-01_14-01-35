using UnityEngine;

public class ItemPickups : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] GameObject Model;
    public string itemName;
    public int price;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnPickup(other.gameObject);
        }
    }

    protected virtual void OnPickup (GameObject player)
    {
        Debug.Log($"{itemName} Picked Up");

        Destroy(gameObject);
    }

}
