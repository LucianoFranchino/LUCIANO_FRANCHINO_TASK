using System;
using System.Collections.Generic;

[System.Serializable]
public class InventoryData 
{
    public List<ItemSlotData> slots = new List<ItemSlotData> ();
}

[System.Serializable]
public class ItemSlotData
{
    public int slotIndex;
    public string itemName; 
    public int count;

    public ItemSlotData(int index, string name, int itemCount)
    {
        slotIndex = index;
        itemName = name;
        count = itemCount;
    }
}
