using Unity.VisualScripting;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;

    public bool AddItem(Item item)
    {
        //coloca el objeto en el mismo slot si es igual
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            DraggableItem itemInSlot = slot.GetComponentInChildren<DraggableItem>();
            if (itemInSlot != null &&
                itemInSlot.item == item &&
                itemInSlot.count < 10 &&
                itemInSlot.item.stackable == true)
            {
                itemInSlot.count++;
                itemInSlot.RefreshCount();
                return true;
            }
        }
        // Busca un slot vacio
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            DraggableItem itemInSlot = slot.GetComponentInChildren<DraggableItem>();
            if(itemInSlot == null)
            {
                SpawnItem(item, slot);
                return true;
            }
        }
        return false;
    }

    void SpawnItem(Item item, InventorySlot slot)
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        DraggableItem inventoryItem = newItemGo.GetComponent<DraggableItem>();
        inventoryItem.InitialiseItem(item);
    }
}
