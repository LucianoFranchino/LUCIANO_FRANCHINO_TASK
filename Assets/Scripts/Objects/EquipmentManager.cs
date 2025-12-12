using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager Instance { get; private set; }

    [Header("Equipment Slots")]
    public Transform equipmentParent;

    [Header("Settings")]
    public float itemScale = 0.5f;

    private GameObject currentEquippedItem; 
    private Item currentEquippedItemData;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (currentEquippedItem != null)
        {
            RotateTowardsMouse();
        }
    }

    public bool EquipItem(Item item)
    {
        if (item.type != ItemType.Tool)
        {
            return false;
        }

        if (currentEquippedItem != null)
        {
            UnequipItem();
        }

        GameObject equippedObject = new GameObject(item.objectName);
        equippedObject.transform.SetParent(equipmentParent);
        equippedObject.transform.localPosition = Vector3.zero;
        equippedObject.transform.localRotation = Quaternion.identity;
        equippedObject.transform.localScale = Vector3.one * itemScale;
        SpriteRenderer sr = equippedObject.AddComponent<SpriteRenderer>();
        sr.sprite = item.image;
        sr.sortingOrder = 6; 

        currentEquippedItem = equippedObject;
        currentEquippedItemData = item;

        Debug.Log("Equipado: " + item.objectName);
        return true;
    }

    public void UnequipItem()
    {
        if (currentEquippedItem != null)
        {
            Destroy(currentEquippedItem);
            currentEquippedItem = null;
            currentEquippedItemData = null;
            Debug.Log("Item desequipado");
        }
    }

    public Item GetCurrentEquippedItem()
    {
        return currentEquippedItemData;
    }

    private void RotateTowardsMouse()
    {
        // Obtener posición del mouse en el mundo
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        // Calcular dirección desde el equipmentParent hacia el mouse
        Vector2 direction = (mousePosition - equipmentParent.position).normalized;

        // Calcular ángulo
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Aplicar rotación al item equipado
        currentEquippedItem.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // Flipear el sprite si apunta a la izquierda
        SpriteRenderer sr = currentEquippedItem.GetComponent<SpriteRenderer>();
        if (direction.x < 0)
        {
            sr.flipY = true;
        }
        else
        {
            sr.flipY = false;
        }
    }
}
