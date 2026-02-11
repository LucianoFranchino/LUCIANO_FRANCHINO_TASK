using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    [Header("Gameplay")]
    public ItemType type;
    public ActionType actionType;
    public Vector2Int range = new Vector2Int(5,4);

    [Header("Tool")]
    public ToolType toolType;

    [Header("UI")]
    public bool stackable = true;
    public string objectName;
    public string description;

    [Header("Both")]
    public Sprite image;
}


public enum ItemType
{
    Tool,
    Consumable,
    Tradding,
    Collectable
}

public enum ActionType
{
    Attack,
    Health,
    Sell,
    Crafting,
    Harvest
}

public enum ToolType
{
    None,
    Axe,
    Pickaxe,
    Weapon
}
