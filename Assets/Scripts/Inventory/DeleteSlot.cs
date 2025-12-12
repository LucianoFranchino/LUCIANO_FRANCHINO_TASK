using UnityEngine;
using UnityEngine.EventSystems;

public class DeleteSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        DraggableItem draggableItem = dropped.GetComponent<DraggableItem>();

        if (draggableItem != null)
        {
            if (EquipmentManager.Instance.GetCurrentEquippedItem() == draggableItem.item)
            {
                EquipmentManager.Instance.UnequipItem();
            }

            Destroy(dropped);
        }
    }
}
