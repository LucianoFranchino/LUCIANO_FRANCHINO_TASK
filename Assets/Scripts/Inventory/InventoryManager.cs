using System;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;
    [SerializeField] private GameObject showInventory;

    [Header("Items Database")]
    public Item[] allItems;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        LoadInventory();
    }
    public void SaveInventory()
    {
        InventoryData data = new InventoryData();

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            DraggableItem itemInSlot = inventorySlots[i].GetComponentInChildren<DraggableItem>();

            if (itemInSlot != null)
            {
                ItemSlotData slotData = new ItemSlotData(
                    i,
                    itemInSlot.item.name,
                    itemInSlot.count
                );
                data.slots.Add(slotData);
            }
        }

        SaveSystem.Instance.SaveInventory(data);
    }
    private void LoadInventory()
    {
        InventoryData data = SaveSystem.Instance.LoadInventory();

        if (data == null) return;
        ClearInventory();

        foreach (ItemSlotData slotData in data.slots)
        {
            Item item = FindItemByName(slotData.itemName);
            if (item != null && slotData.slotIndex < inventorySlots.Length)
            {
                SpawnItem(item, inventorySlots[slotData.slotIndex]);

                DraggableItem draggableItem = inventorySlots[slotData.slotIndex].GetComponentInChildren<DraggableItem>();
                if (draggableItem != null)
                {
                    draggableItem.count = slotData.count;
                    draggableItem.RefreshCount();
                }
            }
        }
    }

    private void ClearInventory()
    {
        foreach (InventorySlot slot in inventorySlots)
        {
            DraggableItem item = slot.GetComponentInChildren<DraggableItem>();
            if (item != null)
            {
                Destroy(item.gameObject);
            }
        }
    }

    private Item FindItemByName(string itemName)
    {
        foreach (Item item in allItems)
        {
            if (item.name == itemName)
            {
                return item;
            }
        }
        Debug.LogWarning("Item no encontrado en database: " + itemName);
        return null;
    }

    private void OnApplicationQuit()
    {
        SaveInventory();
    }
    public bool AddItem(Item item)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            DraggableItem itemInSlot = slot.GetComponentInChildren<DraggableItem>();
            if (itemInSlot != null &&
                itemInSlot.item == item &&
                itemInSlot.count < 5 &&
                itemInSlot.item.stackable == true)
            {
                itemInSlot.count++;
                itemInSlot.RefreshCount();
                return true;
            }
        }

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            showInventory.SetActive(!showInventory.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.F5))
        {
            SaveInventory();
        }

        if (Input.GetKeyDown(KeyCode.F9))
        {
            LoadInventory();
        }
    }
}
