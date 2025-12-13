using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public Item item;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            bool wasAdded = InventoryManager.Instance.AddItem(item);

            if (wasAdded) Destroy(gameObject);
        }
    }
}
