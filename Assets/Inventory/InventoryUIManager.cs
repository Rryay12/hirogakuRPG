using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class InventoryUIManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject inventoryPanel;

    [Header("Item List")]
    public Transform itemListContent;
    public GameObject itemSlotPrefab;

    [Header("Item Description")]
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescriptionText;
    public TextMeshProUGUI itemStatsText;
    public Image itemIconImage;
    public Button useEquipButton;
    
    [Header("Equipment Slots")]
    public Transform equipmentSlotsParent;

    private InventoryManager inventoryManager;
    private EquipmentManager equipmentManager;
    private InventoryItem selectedItem;

    void Start()
    {
        inventoryManager = FindFirstObjectByType<InventoryManager>();
        equipmentManager = EquipmentManager.Instance;

        if (inventoryManager == null || equipmentManager == null)
        {
            Debug.LogError("A required manager was not found! The UI cannot function.");
            this.enabled = false;
            return;
        }

        if (inventoryPanel == null || itemSlotPrefab == null)
        {
            Debug.LogError("A required UI Prefab/Panel is not assigned in the Inspector! The UI cannot function.");
            this.enabled = false;
            return;
        }
        
        inventoryManager.OnInventoryChanged += UpdateItemList;
        equipmentManager.OnEquipmentChanged += UpdateEquipmentSlots;
        useEquipButton.onClick.AddListener(OnUseEquipButtonClick);

        ClearDescription();
        inventoryPanel.SetActive(false);
    }
    
    public void OnToggleInventory(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            bool isCurrentlyActive = inventoryPanel.activeSelf;
            inventoryPanel.SetActive(!isCurrentlyActive);

            if (inventoryPanel.activeSelf)
            {
                UpdateItemList();
            }
        }
    }

    private void OnDestroy()
    {
        if (inventoryManager != null) { inventoryManager.OnInventoryChanged -= UpdateItemList; }
        if (equipmentManager != null) { equipmentManager.OnEquipmentChanged -= UpdateEquipmentSlots; }
    }

    void UpdateItemList()
    {
        foreach (Transform child in itemListContent) { Destroy(child.gameObject); }
        if (itemSlotPrefab == null) { Debug.LogError("Item Slot Prefab is not assigned!"); return; }

        List<InventoryItem> items = inventoryManager.GetAllItems();
        foreach (var item in items)
        {
            GameObject slotGO = Instantiate(itemSlotPrefab, itemListContent);
            ItemSlot slot = slotGO.GetComponent<ItemSlot>();
            if (slot != null) { slot.Setup(item, this); }
            else { Debug.LogError("Instantiated Item Slot Prefab is missing the 'ItemSlot' script!"); }
        }
    }

    public void SelectItem(InventoryItem item) 
    { 
        selectedItem = item; 
        UpdateDescription(); 
    }

    void UpdateDescription() 
    {
        if (selectedItem != null) 
        {
            itemNameText.text = selectedItem.itemData.itemName; 
            itemDescriptionText.text = selectedItem.itemData.description; 
            itemIconImage.sprite = Resources.Load<Sprite>(selectedItem.itemData.iconPath); 
            itemIconImage.color = itemIconImage.sprite == null ? Color.clear : Color.white; 
            itemStatsText.text = GetStatsText(selectedItem.itemData.boost); 
            useEquipButton.gameObject.SetActive(true); 
            TextMeshProUGUI buttonText = useEquipButton.GetComponentInChildren<TextMeshProUGUI>(); 
            if (selectedItem.itemData.isEquippable) 
            { 
                buttonText.text = "Equip"; 
            } 
            else if (selectedItem.itemData.isConsumable) 
            { 
                buttonText.text = "Use"; 
            } 
            else 
            { 
                useEquipButton.gameObject.SetActive(false); 
            } 
        } 
        else 
        { 
            ClearDescription(); 
        } 
    }

    void ClearDescription() 
    { 
        selectedItem = null; 
        itemNameText.text = ""; 
        itemDescriptionText.text = ""; 
        itemStatsText.text = ""; 
        itemIconImage.sprite = null; 
        itemIconImage.color = Color.clear; 
        useEquipButton.gameObject.SetActive(false); 
    }
    
    string GetStatsText(Boost boost) 
    { 
        string stats = ""; 
        if (boost.Hp.Hp != 0) stats += $"HP: +{boost.Hp.Hp}\n"; 
        if (boost.Attack.phyAttack != 0) stats += $"Attack: +{boost.Attack.phyAttack}\n"; 
        return stats; 
    }
    
    void OnUseEquipButtonClick() 
    { 
        if (selectedItem == null) return; 

        if (selectedItem.itemData.isEquippable) 
        { 
            equipmentManager.EquipItem(selectedItem); 
        } 
        else if (selectedItem.itemData.isConsumable) 
        { 
            inventoryManager.RemoveItem(selectedItem.itemData.itemId, 1); 
            ClearDescription(); 
        } 
    }
    
    void UpdateEquipmentSlots(EquipmentSlotType updatedSlotType) { }
}
