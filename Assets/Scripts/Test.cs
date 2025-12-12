using UnityEngine;

public class Test : MonoBehaviour
{
   public InventoryManager inventoryManager;
   public Item[] itemsPickUps;

    public void PickUpItem(int id)
    {
        bool result = inventoryManager.AddItem(itemsPickUps[id]);
        if (result == true)
        {
            Debug.Log("Agregado!");
        }
        else
        {
            Debug.Log("No hay espacio negro");
        }
    }

}
