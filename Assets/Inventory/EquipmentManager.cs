using UnityEngine;
using System.Collections.Generic;
using System;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager Instance { get; private set; }
    
    public event Action<EquipmentSlotType> OnEquipmentChanged;

    private Dictionary<EquipmentSlotType, InventoryItem> equippedItems = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        foreach (EquipmentSlotType slotType in Enum.GetValues(typeof(EquipmentSlotType)))
        {
            equippedItems.Add(slotType, null);
        }
    }

    public void EquipItem(InventoryItem itemToEquip)
    {
        if (itemToEquip == null || !itemToEquip.itemData.isEquippable) return;

        if (Enum.TryParse(itemToEquip.itemData.itemType, true, out EquipmentSlotType slotType))
        {            if (equippedItems[slotType] != null)
            {
                UnequipItem(slotType);
            }

            equippedItems[slotType] = itemToEquip;
            Debug.Log($"Equipped {itemToEquip.itemData.itemName} to {slotType} slot.");
            OnEquipmentChanged?.Invoke(slotType);
        }
        else
        {
            Debug.LogWarning($"Could not find a matching EquipmentSlotType for item type '{itemToEquip.itemData.itemType}'.");
        }
    }

    public void UnequipItem(EquipmentSlotType slotType)
    {
        if (equippedItems[slotType] != null)
        {   
            Debug.Log($"Unequipped {equippedItems[slotType].itemData.itemName} from {slotType} slot.");
            equippedItems[slotType] = null;
            OnEquipmentChanged?.Invoke(slotType);
        }
    }
    
    public InventoryItem GetEquippedItem(EquipmentSlotType slotType)
    {
        return equippedItems[slotType];
    }
}

public enum EquipmentSlotType
{
    Weapon,
    Helmet,
    Armor,
    Boots,
    Accessory
}