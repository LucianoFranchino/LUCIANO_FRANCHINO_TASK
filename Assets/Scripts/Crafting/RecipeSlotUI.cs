using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RecipeSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Referencias UI")]
    [SerializeField] private Image itemIcon;
    [SerializeField] private Image slotBackground;

    [Header("Colores")]
    [SerializeField] private Color normalColor = new Color(0.2f, 0.2f, 0.2f, 0.9f);
    [SerializeField] private Color hoverColor = new Color(0.35f, 0.35f, 0.35f, 1f);
    [SerializeField] private Color cantCraftColor = new Color(0.5f, 0.15f, 0.15f, 0.7f);

    private CraftingRecipe recipe;

    public void Setup(CraftingRecipe recipeData)
    {
        recipe = recipeData;
        itemIcon.sprite = recipeData.resultItem.image;
        RefreshVisual();
    }

    // Llámalo desde fuera si el inventario cambia, para actualizar el color
    public void RefreshVisual()
    {
        bool canCraft = CraftingManager.Instance.CanCraft(recipe);
        slotBackground.color = canCraft ? normalColor : cantCraftColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        slotBackground.color = hoverColor;
        CraftingTooltipUI.Instance.Show(recipe);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        RefreshVisual();
        CraftingTooltipUI.Instance.Hide();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        bool success = CraftingManager.Instance.TryCraft(recipe);
        if (success)
        {
            RefreshVisual();
        }
        else
        {
            StartCoroutine(ShakeSlot());
        }
    }

    // Feedback visual si no se puede craftear
    private System.Collections.IEnumerator ShakeSlot()
    {
        Vector3 originalPos = transform.localPosition;
        float shakeAmount = 5f;
        float duration = 0.3f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = originalPos.x + Random.Range(-shakeAmount, shakeAmount);
            transform.localPosition = new Vector3(x, originalPos.y, originalPos.z);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
