using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class StoreItems : MonoBehaviour
{
    public static StoreItems Instance { get; private set; }
    public Dictionary<string, ItemData> itemsByID = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadItems();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadItems()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "items.json");
        if (!File.Exists(path))
        {
            Debug.LogError("items.json not found at: " + path);
            return;
        }

        string json = File.ReadAllText(path);
        ItemData[] items = JsonHelper.FromJson<ItemData>(json);

        foreach (ItemData item in items)
        {
            item.icon = Resources.Load<Sprite>(item.iconPath);
            itemsByID[item.itemId] = item;
        }

        Debug.Log("Loaded " + itemsByID.Count + " items.");
    }

    public ItemData GetItemByID(string itemId)
    {
        itemsByID.TryGetValue(itemId, out var item);
        return item;
    }

    public List<ItemData> GetAllItems()
    {
        return new List<ItemData>(itemsByID.Values);
    }
}

[System.Serializable]
public class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        return JsonUtility.FromJson<Wrapper<T>>("{\"Items\":" + json + "}").Items;
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}