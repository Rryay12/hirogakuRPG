using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
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
                Debug.Log($"Added {amount} to '{data.itemName}'. New total: {inventory[itemId].quantity}.");
            }
            else
            {
                Debug.LogWarning($"Tried to stack unstackable item: '{data.itemName}'.");
            }
        }
        else
        {
            inventory[itemId] = new InventoryItem(data, amount);
            Debug.Log($"Added new item to inventory: '{data.itemName}'.");
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
