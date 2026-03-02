using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingTooltipUI : MonoBehaviour
{
    public static CraftingTooltipUI Instance { get; private set; }

    [Header("Referencias UI")]
    [SerializeField] private GameObject tooltipPanel;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI ingredientsText;
    [SerializeField] private RectTransform tooltipRect;

    [Header("Offset del mouse")]
    [SerializeField] private Vector2 offset = new Vector2(15f, -15f);

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        tooltipPanel.SetActive(false);
    }

    private void Update()
    {
        if (tooltipPanel.activeSelf)
            FollowMouse();
    }

    public void Show(CraftingRecipe recipe)
    {
        itemNameText.text = recipe.resultItem.objectName;
        descriptionText.text = recipe.resultItem.description;
        ingredientsText.text = BuildIngredientText(recipe);

        tooltipPanel.SetActive(true);
        FollowMouse();
    }

    public void Hide()
    {
        tooltipPanel.SetActive(false);
    }

    private string BuildIngredientText(CraftingRecipe recipe)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"Requiere {recipe.itemAmount} de:");

        foreach (var ingredient in recipe.requiredIngredient)
        {
            int playerCount = CraftingManager.Instance.GetItemCount(ingredient.item.objectName);
            bool hasEnough = playerCount >= ingredient.amount;

            // Color verde si tiene suficiente, rojo si no
            string color = hasEnough ? "#90EE90" : "#FF6B6B";
            sb.AppendLine($"<color={color}>{playerCount}/{ingredient.amount} {ingredient.item.objectName}</color>");
        }

        return sb.ToString().TrimEnd();
    }

    private void FollowMouse()
    {
        Vector2 mousePos = Input.mousePosition;

        // Convertir posición del mouse a coordenadas locales del Canvas
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            tooltipRect.parent as RectTransform,
            mousePos,
            null, // null si el Canvas es Screen Space Overlay, si no pasá la Camera
            out Vector2 localPoint
        );

        // Aplicar offset
        tooltipRect.localPosition = localPoint + offset;

        // Evitar que se salga por la derecha
        Vector2 pos = tooltipRect.localPosition;
        float rightEdge = pos.x + tooltipRect.rect.width;
        float canvasWidth = (tooltipRect.parent as RectTransform).rect.width;

        if (rightEdge > canvasWidth / 2f)
            pos.x -= tooltipRect.rect.width + Mathf.Abs(offset.x) * 2;

        // Evitar que se salga por arriba
        float topEdge = pos.y + tooltipRect.rect.height;
        float canvasHeight = (tooltipRect.parent as RectTransform).rect.height;

        if (topEdge > canvasHeight / 2f)
            pos.y -= tooltipRect.rect.height + Mathf.Abs(offset.y) * 2;

        tooltipRect.localPosition = pos;
    }
}
