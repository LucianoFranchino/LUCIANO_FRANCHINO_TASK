using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class ItemInfoPanel : MonoBehaviour
{
    public static ItemInfoPanel Instance { get; private set; }

    [Header("UI References")]
    public Image itemImage;
    public TMP_Text itemName;
    public TMP_Text itemDescription;

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
    public void ShowItemInfo(Item item)
    {
        itemImage.sprite = item.image;
        itemName.text = item.objectName;
        itemDescription.text = item.description;
    }
}
