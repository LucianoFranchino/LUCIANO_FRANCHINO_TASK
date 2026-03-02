using UnityEngine;

public class CraftingPanelUI : MonoBehaviour
{
    [SerializeField] private Transform contentParent;       // El Content del ScrollRect
    [SerializeField] private RecipeSlotUI recipeSlotPrefab; // Prefab del slot

    private RecipeSlotUI[] activeSlots;

    public void BuildPanel(CraftingRecipe[] recipes)
    {
        Debug.Log($"Panel con {recipes.Length}");
        // Limpiar slots viejos si los hay
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        activeSlots = new RecipeSlotUI[recipes.Length];

        for (int i = 0; i < recipes.Length; i++)
        {
            RecipeSlotUI slot = Instantiate(recipeSlotPrefab, contentParent);
            slot.Setup(recipes[i]);
            activeSlots[i] = slot;
        }
    }

    // Llámalo cuando el inventario cambie para actualizar colores
    public void RefreshAllSlots()
    {
        if (activeSlots == null) return;
        foreach (var slot in activeSlots)
            slot.RefreshVisual();
    }
}
