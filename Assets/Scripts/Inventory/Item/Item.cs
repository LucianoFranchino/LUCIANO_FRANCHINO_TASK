using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    [Header("Gameplay")]
    public ItemType type;
    public ActionType actionType;
    public Vector2Int range = new Vector2Int(5,4);

    [Header("UI")]
    public bool stackable = true;

    [Header("Both")]
    public Sprite image;
}


public enum ItemType
{
    Tool,
    Consumable,
    Tradding,
    Drop
}

public enum ActionType
{
    Attack,
    Health,
    Sell
}
