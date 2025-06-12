using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlot : MonoBehaviour
{
    [Header("UI Elements")]
    public Image iconImage;
    public TextMeshProUGUI quantityText;
    public Button slotButton;

    private InventoryItem item;
    private InventoryUIManager uiManager;

    public void Setup(InventoryItem newItem, InventoryUIManager manager)
    {
        item = newItem;
        uiManager = manager;

        if (item != null)
        {
            iconImage.sprite = Resources.Load<Sprite>(item.itemData.iconPath);
            iconImage.enabled = true;

            if (item.itemData.isStackable && item.quantity > 1)
            {
                quantityText.text = item.quantity.ToString();
                quantityText.enabled = true;
            }
            else
            {
                quantityText.enabled = false;
            }
        }
        
        slotButton.onClick.AddListener(OnSlotClicked);
    }
    
    void OnSlotClicked()
    {
        if (item != null && uiManager != null)
        {
            uiManager.SelectItem(item);
        }
    }
}
