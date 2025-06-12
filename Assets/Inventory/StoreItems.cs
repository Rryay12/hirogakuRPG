using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class StoreItems : MonoBehaviour
{
    public static StoreItems Instance { get; private set; }

    private Dictionary<string, ItemData> itemDatabase = new();

    public string jsonFileName = "items";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadItemData();
    }

    private void LoadItemData()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(jsonFileName);
        if (jsonFile == null)
        {
            Debug.LogError($"Cannot find '{jsonFileName}.json' in any Resources folder. Please create a 'Resources' folder and place the file there.");
            return;
        }

        ItemData[] items = JsonHelper.FromJson<ItemData>(jsonFile.text);

        foreach (var item in items)
        {
            if (!itemDatabase.ContainsKey(item.itemId))
            {
                itemDatabase.Add(item.itemId, item);
            }
            else
            {
                Debug.LogWarning($"Duplicate item ID found in JSON: {item.itemId}. The first item was kept.");
            }
        }

        Debug.Log($"Successfully loaded {itemDatabase.Count} items from JSON.");
    }

    public ItemData GetItemByID(string itemId)
    {
        itemDatabase.TryGetValue(itemId, out ItemData item);
        if (item == null)
        {
            Debug.LogWarning($"Item with ID '{itemId}' not found in the database.");
        }
        return item;
    }
}


public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}
