using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    public static CraftingManager Instance { get; private set; }

    [Header("Recetas disponibles")]
    [SerializeField] private CraftingRecipe[] recipes;

    [Header("UI")]
    [SerializeField] private CraftingPanelUI craftingPanelUI;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void Start()
    {
        craftingPanelUI.BuildPanel(recipes);
    }

    // Devuelve cuántos tiene el jugador de un item específico
    public int GetItemCount(string itemName)
    {
        return InventoryManager.Instance.GetItemCount(itemName);
    }

    // Verifica si el jugador puede craftear una receta
    public bool CanCraft(CraftingRecipe recipe)
    {
        foreach (var ingredient in recipe.requiredIngredient)
        {
            if (GetItemCount(ingredient.item.objectName) < ingredient.amount)
                return false;
        }
        return true;
    }

    // Intenta craftear. Retorna true si tuvo éxito.
    public bool TryCraft(CraftingRecipe recipe)
    {
        if (!CanCraft(recipe)) return false;

        // Quitar ingredientes
        foreach (var ingredient in recipe.requiredIngredient)
            InventoryManager.Instance.RemoveItem(ingredient.item.objectName, ingredient.amount);

        // Agregar resultado
        InventoryManager.Instance.AddItem(recipe.resultItem, recipe.itemAmount);

        return true;
    }

    public CraftingRecipe[] GetAllRecipes() => recipes;
}
