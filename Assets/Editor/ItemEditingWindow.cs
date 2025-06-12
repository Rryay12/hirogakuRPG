#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class ItemEditorWindow : EditorWindow
{
    private List<ItemData> items = new();
    private Vector2 scrollPos;
    private string jsonFilePath = "Assets/Resources/items.json"; 

    [MenuItem("Tools/RPG/Item Editor")]
    public static void ShowWindow()
    {
        GetWindow<ItemEditorWindow>("Item Editor");
    }

    private void OnEnable()
    {
        LoadItems(); 
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Item Database Editor", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add New Item"))
        {
            items.Add(new ItemData
            {
                itemId = System.Guid.NewGuid().ToString("N"),
                itemName = "New Item",
                boost = new Boost
                {
                    Hp = new HpBoost(),
                    Mana = new ManaBoost(),
                    Attack = new AttackBoost(),
                    Defence = new DefenceBoost()
                }
            });
        }

        if (GUILayout.Button("Save to JSON"))
        {
            SaveItems();
        }
        
        if (GUILayout.Button("Load from JSON"))
        {
            LoadItems();
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        for (int i = 0; i < items.Count; i++)
        {
            var item = items[i];
            EditorGUILayout.BeginVertical(EditorStyles.helpBox); 

            EditorGUILayout.BeginHorizontal();
            item.itemName = EditorGUILayout.TextField("Item Name", item.itemName);
            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("Remove", GUILayout.Width(70)))
            {
                if (EditorUtility.DisplayDialog("Delete Item?", $"Are you sure you want to delete '{item.itemName}'?", "Yes", "No"))
                {
                    items.RemoveAt(i);
                    continue; 
                }
            }
            GUI.backgroundColor = Color.white;
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField("ID", item.itemId);
            

            item.iconPath = EditorGUILayout.TextField("Icon Path (in Resources)", item.iconPath);
            EditorGUILayout.LabelField("Description");
            item.description = EditorGUILayout.TextArea(item.description, GUILayout.Height(40));
            item.itemType = EditorGUILayout.TextField("Type", item.itemType);
            item.isConsumable = EditorGUILayout.Toggle("Consumable", item.isConsumable);
            item.isEquippable = EditorGUILayout.Toggle("Equippable", item.isEquippable);
            item.isStackable = EditorGUILayout.Toggle("Stackable", item.isStackable);
            item.dropChance = EditorGUILayout.Slider("Drop Chance", item.dropChance, 0f, 1f);
            item.rarity = EditorGUILayout.TextField("Rarity", item.rarity);

            EditorGUILayout.LabelField("Boost Stats", EditorStyles.boldLabel);
            item.boost.Hp.maxHP = EditorGUILayout.IntField("Max HP", item.boost.Hp.maxHP);
            item.boost.Hp.Hp = EditorGUILayout.IntField("HP", item.boost.Hp.Hp);
            item.boost.Mana.maxMana = EditorGUILayout.IntField("Max Mana", item.boost.Mana.maxMana);
            item.boost.Mana.Mana = EditorGUILayout.IntField("Mana", item.boost.Mana.Mana);
            item.boost.Attack.phyAttack = EditorGUILayout.IntField("Physical Attack", item.boost.Attack.phyAttack);
            item.boost.Attack.magAttack = EditorGUILayout.IntField("Magic Attack", item.boost.Attack.magAttack);
            item.boost.Defence.phyDefence = EditorGUILayout.IntField("Physical Defence", item.boost.Defence.phyDefence);
            item.boost.Defence.magDefence = EditorGUILayout.IntField("Magic Defence", item.boost.Defence.magDefence);
            item.boost.charisma = EditorGUILayout.IntField("Charisma", item.boost.charisma);
            item.boost.spd = EditorGUILayout.IntField("Speed", item.boost.spd);
            
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
        }

        EditorGUILayout.EndScrollView();
    }

    private void SaveItems()
    {
        string directoryPath = Path.GetDirectoryName(jsonFilePath);
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        ItemListWrapper wrapper = new ItemListWrapper { Items = items.ToArray() };
        string json = JsonUtility.ToJson(wrapper, true); 
        File.WriteAllText(jsonFilePath, json);
        Debug.Log($"Exported {items.Count} items to {jsonFilePath}");
        AssetDatabase.Refresh(); 
    }

    private void LoadItems()
    {
        if (File.Exists(jsonFilePath))
        {
            string json = File.ReadAllText(jsonFilePath);
            ItemListWrapper wrapper = JsonUtility.FromJson<ItemListWrapper>(json);
            if (wrapper != null && wrapper.Items != null)
            {
                items = wrapper.Items.ToList();
                Debug.Log($"Loaded {items.Count} items from {jsonFilePath}");
            }
            else
            {
                items = new List<ItemData>();
                Debug.LogWarning($"Could not parse items from {jsonFilePath}. File might be empty or corrupt.");
            }
        }
        else
        {
            items = new List<ItemData>(); 
            Debug.Log($"JSON file not found at {jsonFilePath}. Starting with a new item list.");
        }
    }

    [System.Serializable]
    private class ItemListWrapper
    {
        public ItemData[] Items;
    }
}
#endif
