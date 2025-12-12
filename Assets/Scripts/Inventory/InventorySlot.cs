using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        DraggableItem draggableItem = dropped.GetComponent<DraggableItem>();

        if (transform.childCount == 0)
        {
            draggableItem.parentAfterDrag = transform;
        }
        else
        {
            DraggableItem itemInSlot = transform.GetChild(0).GetComponent<DraggableItem>();
            Transform originalParent = draggableItem.parentAfterDrag;
            draggableItem.parentAfterDrag = transform;
            itemInSlot.transform.SetParent(originalParent);
        }
    }

}
