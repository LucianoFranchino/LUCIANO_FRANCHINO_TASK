using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public Item item;
    [SerializeField] AudioClip coinSound;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            bool wasAdded = InventoryManager.Instance.AddItem(item);
            if(item.objectName == "Diamont")
            {
                AudioManager.instance.PlayAudio(coinSound);
            }
            if (wasAdded) Destroy(gameObject);
        }
    }
}
