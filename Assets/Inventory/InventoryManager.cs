using UnityEngine;
using System.Collections.Generic;
using System;
public class InventoryManager : MonoBehaviour
{
    public event Action OnInventoryChanged;

    private Dictionary<string, InventoryItem> inventory = new();

    public void AddItem(string itemId, int amount = 1)
    {
        ItemData data = StoreItems.Instance.GetItemByID(itemId);
        if (data == null)
        {
            Debug.LogWarning("Attempted to add an item with an invalid ID: " + itemId);
            return;
        }

        if (inventory.ContainsKey(itemId))
        {
            if (data.isStackable)
            {
                inventory[itemId].quantity += amount;
            }
            else
            {
                Debug.Log($"Cannot stack unstackable item: '{data.itemName}'. Item not added.");
            }
        }
        else
        {
            inventory[itemId] = new InventoryItem(data, amount);
        }

        OnInventoryChanged?.Invoke();
    }

    public void RemoveItem(string itemId, int amount = 1)
    {
        if (inventory.TryGetValue(itemId, out InventoryItem item))
        {
            item.quantity -= amount;
            if (item.quantity <= 0)
            {
                inventory.Remove(itemId);
            }

            OnInventoryChanged?.Invoke();
        }
    }

    public InventoryItem GetItem(string itemId)
    {
        inventory.TryGetValue(itemId, out InventoryItem item);
        return item;
    }

    public List<InventoryItem> GetAllItems()
    {
        return new List<InventoryItem>(inventory.Values);
    }
}

[System.Serializable]
public class InventoryItem
{
    public ItemData itemData;
    public int quantity;

    public InventoryItem(ItemData data, int quantity)
    {
        this.itemData = data;
        this.quantity = quantity;
    }
}
