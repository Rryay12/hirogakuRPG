#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class ItemEditorWindow : EditorWindow
{
    private List<ItemData> items = new();
    private Vector2 scrollPos;

    [MenuItem("Tools/Item Editor")]
    public static void ShowWindow()
    {
        GetWindow<ItemEditorWindow>("Item Editor");
    }

    private void OnGUI()
    {
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

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        foreach (var item in items)
        {
            item.itemId = EditorGUILayout.TextField("ID", item.itemId);
            item.itemName = EditorGUILayout.TextField("Name", item.itemName);
            item.iconPath = EditorGUILayout.TextField("Icon Path", item.iconPath);
            item.description = EditorGUILayout.TextField("Description", item.description);
            item.itemType = EditorGUILayout.TextField("Type", item.itemType);
            item.isConsumable = EditorGUILayout.Toggle("Consumable", item.isConsumable);
            item.isEquippable = EditorGUILayout.Toggle("Equippable", item.isEquippable);
            item.isStackable = EditorGUILayout.Toggle("Stackable", item.isStackable);
            item.dropChance = EditorGUILayout.FloatField("Drop Chance", item.dropChance);
            item.rarity = EditorGUILayout.TextField("Rarity", item.rarity);

            EditorGUILayout.LabelField("Boost Stats:");
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

            EditorGUILayout.Space();
        }

        EditorGUILayout.EndScrollView();

        if (GUILayout.Button("Export to JSON"))
        {
            string path = Path.Combine(Application.streamingAssetsPath, "items.json");
            string json = JsonUtility.ToJson(new ItemListWrapper { Items = items.ToArray() }, true);
            File.WriteAllText(path, json);
            Debug.Log("Exported to " + path);
        }
    }

    [System.Serializable]
    private class ItemListWrapper
    {
        public ItemData[] Items;
    }
}
#endif
