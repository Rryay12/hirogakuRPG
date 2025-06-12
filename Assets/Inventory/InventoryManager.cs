using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    private Dictionary<string, InventoryItem> inventory = new();

    public void AddItem(string itemId, int amount = 1)
    {
        ItemData data = StoreItems.Instance.GetItem(itemId);
        if (data == null)
        {
            Debug.LogWarning("Item ID not found: " + itemId);
            return;
        }

        if (inventory.ContainsKey(itemId))
        {
            inventory[itemId].quantity += amount;
        }
        else
        {
            inventory[itemId] = new InventoryItem(data, amount);
        }
    }

    public InventoryItem GetItem(string itemId)
    {
        inventory.TryGetValue(itemId, out InventoryItem item);
        return item;
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