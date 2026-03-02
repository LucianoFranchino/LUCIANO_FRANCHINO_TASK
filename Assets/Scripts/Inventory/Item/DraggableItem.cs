using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler//, IPointerEnterHandler
{
    [Header("UI")]
    public Image image;
    public Text countText;

    [HideInInspector] public Transform parentAfterDrag;
    [HideInInspector] public Item item;
    [HideInInspector]public int count = 1;

    private CraftingRecipe recipe;

    public void InitialiseItem(Item newItem)
    {
        item = newItem;
        image.sprite = newItem.image;
        RefreshCount();
    }

    public void RefreshCount()
    {
        countText.text = count.ToString();
        bool textActive = count > 1;
        countText.gameObject.SetActive(textActive);
        countText.raycastTarget = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;

    }
    //public void OnPointerEnter(PointerEventData eventData)
    //{
    //    CraftingTooltipUI.Instance.Show(recipe);
    //}

    //public void OnPointerExit(PointerEventData eventData)
    //{
    //    CraftingTooltipUI.Instance.Hide();
    //}

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            UseItem();
        }
    }

    private void UseItem()
    {
        if (item.type == ItemType.Tool)
        {
            EquipmentManager.Instance.EquipItem(item);
        }
        else if (item.type == ItemType.Consumable)
        {
            
            if (item.actionType == ActionType.Health)
            {
                PlayerHealth playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.Heal(item.range.x);
                }
            }
            count--;
            if (count <= 0)
            {
                if (EquipmentManager.Instance.GetCurrentEquippedItem() == item)
                {
                    EquipmentManager.Instance.UnequipItem();
                }

                Destroy(gameObject);
            }
            else
            {
                RefreshCount();
            }
        }
    }

   

}
