using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

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
        Debug.Log("--- BEGINNING FINAL SCRIPT DEBUG ---");
        
        // --- STEP 1: Verify all Inspector assignments ---
        bool allAssigned = true;
        if (inventoryPanel == null) { Debug.LogError("FINAL DEBUG: inventoryPanel IS NULL!"); allAssigned = false; }
        if (itemListContent == null) { Debug.LogError("FINAL DEBUG: itemListContent IS NULL!"); allAssigned = false; }
        if (itemSlotPrefab == null) { Debug.LogError("FINAL DEBUG: itemSlotPrefab IS NULL!"); allAssigned = false; }
        if (itemNameText == null) { Debug.LogError("FINAL DEBUG: itemNameText IS NULL!"); allAssigned = false; }
        if (itemDescriptionText == null) { Debug.LogError("FINAL DEBUG: itemDescriptionText IS NULL!"); allAssigned = false; }
        if (itemStatsText == null) { Debug.LogError("FINAL DEBUG: itemStatsText IS NULL!"); allAssigned = false; }
        if (itemIconImage == null) { Debug.LogError("FINAL DEBUG: itemIconImage IS NULL!"); allAssigned = false; }
        if (useEquipButton == null) { Debug.LogError("FINAL DEBUG: useEquipButton IS NULL!"); allAssigned = false; }
        
        if (!allAssigned)
        {
            Debug.LogError("FINAL DEBUG: At least one variable is not assigned in the Inspector. Script is now disabled to prevent further errors.");
            this.enabled = false; // Disable this script
            return;
        }
        Debug.Log("FINAL DEBUG: Step 1 PASSED - All Inspector variables are assigned.");

        // --- STEP 2: Find the manager scripts ---
        inventoryManager = FindFirstObjectByType<InventoryManager>();
        equipmentManager = EquipmentManager.Instance;
        if (inventoryManager == null || equipmentManager == null)
        {
            Debug.LogError("FINAL DEBUG: Could not find InventoryManager or EquipmentManager. Script is now disabled.");
            this.enabled = false;
            return;
        }
        Debug.Log("FINAL DEBUG: Step 2 PASSED - All managers found.");

        // --- STEP 3: Subscribe to events ---
        inventoryManager.OnInventoryChanged += UpdateItemList;
        equipmentManager.OnEquipmentChanged += UpdateEquipmentSlots;
        useEquipButton.onClick.AddListener(OnUseEquipButtonClick);
        Debug.Log("FINAL DEBUG: Step 3 PASSED - Subscribed to events.");
        
        // --- STEP 4: Initialize the UI state ---
        ClearDescription();
        inventoryPanel.SetActive(false);
        
        Debug.Log("--- FINAL DEBUG: START FUNCTION COMPLETED SUCCESSFULLY. SCRIPT IS NOW WAITING FOR INPUT. ---");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("FINAL DEBUG: 'I' Key Press was DETECTED!");
            
            bool isCurrentlyActive = inventoryPanel.activeSelf;
            inventoryPanel.SetActive(!isCurrentlyActive);

            // If we just opened the panel, refresh the item list.
            if (inventoryPanel.activeSelf)
            {
                Debug.Log("FINAL DEBUG: Panel is now active. Calling UpdateItemList().");
                UpdateItemList();
            }
        }
    }
    
    public void TogglePanelVisibility()
    {
        Debug.Log("BUTTON CLICKED! Toggling panel visibility.");
        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        }
    }
    void UpdateItemList()
    {
        Debug.Log("...UpdateItemList: Clearing old items...");
        foreach (Transform child in itemListContent)
        {
            Destroy(child.gameObject);
        }

        List<InventoryItem> items = inventoryManager.GetAllItems();
        Debug.Log($"...UpdateItemList: Found {items.Count} items in inventory. Populating list...");

        foreach (var item in items)
        {
            Debug.Log($"......Creating slot for item: {item.itemData.itemName}");
            GameObject slotGO = Instantiate(itemSlotPrefab, itemListContent);
            
            if (slotGO == null)
            {
                Debug.LogError($"......FAILED to Instantiate itemSlotPrefab for {item.itemData.itemName}!");
                continue; 
            }

            ItemSlot slot = slotGO.GetComponent<ItemSlot>();
            if (slot == null)
            {
                Debug.LogError($"......Instantiated prefab for {item.itemData.itemName} is MISSING the ItemSlot script!");
                continue;
            }
            
            slot.Setup(item, this);
            Debug.Log($"......Successfully created slot for {item.itemData.itemName}");
        }
    }

    // --- Other functions remain the same ---
    private void OnDestroy() { if (inventoryManager != null) { inventoryManager.OnInventoryChanged -= UpdateItemList; } if (equipmentManager != null) { equipmentManager.OnEquipmentChanged -= UpdateEquipmentSlots; } }
    public void SelectItem(InventoryItem item) { selectedItem = item; UpdateDescription(); }
    void UpdateDescription() { if (selectedItem != null) { itemNameText.text = selectedItem.itemData.itemName; itemDescriptionText.text = selectedItem.itemData.description; itemIconImage.sprite = Resources.Load<Sprite>(selectedItem.itemData.iconPath); itemIconImage.color = itemIconImage.sprite == null ? Color.clear : Color.white; itemStatsText.text = GetStatsText(selectedItem.itemData.boost); useEquipButton.gameObject.SetActive(true); TextMeshProUGUI buttonText = useEquipButton.GetComponentInChildren<TextMeshProUGUI>(); if (selectedItem.itemData.isEquippable) { buttonText.text = "Equip"; } else if (selectedItem.itemData.isConsumable) { buttonText.text = "Use"; } else { useEquipButton.gameObject.SetActive(false); } } else { ClearDescription(); } }
    void ClearDescription() { selectedItem = null; itemNameText.text = ""; itemDescriptionText.text = ""; itemStatsText.text = ""; itemIconImage.sprite = null; itemIconImage.color = Color.clear; useEquipButton.gameObject.SetActive(false); }
    string GetStatsText(Boost boost) { string stats = ""; if (boost.Hp.Hp != 0) stats += $"HP: +{boost.Hp.Hp}\n"; if (boost.Attack.phyAttack != 0) stats += $"Attack: +{boost.Attack.phyAttack}\n"; return stats; }
    void OnUseEquipButtonClick() { if (selectedItem == null) return; if (selectedItem.itemData.isEquippable) { equipmentManager.EquipItem(selectedItem); } else if (selectedItem.itemData.isConsumable) { inventoryManager.RemoveItem(selectedItem.itemData.itemId, 1); ClearDescription(); } }
    void UpdateEquipmentSlots(EquipmentSlotType updatedSlotType) { }
}