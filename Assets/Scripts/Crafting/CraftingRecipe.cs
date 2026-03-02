using UnityEngine;

[CreateAssetMenu(fileName = "CraftingRecipe", menuName = "Scriptable Objects/CraftingRecipe")]
public class CraftingRecipe : ScriptableObject
{
    [Header("Item")]
    public Item resultItem;
    public int itemAmount = 1;

    [Header("Ingredients")]
    public Ingredient[] requiredIngredient;

}

[System.Serializable]
public class Ingredient
{
    public Item item;
    public int amount;
}
