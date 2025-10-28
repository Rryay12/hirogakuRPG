using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;


public class ItemInventory
{
    public SaveManager manager = new SaveManager();

    public Item getItem(string name)
    {
        string path = Application.dataPath + "/itemJson/" + name + ".json";
        Debug.Log(path);
        Item newItem = manager.load<Item>(path);
        return newItem;
    }

}


public class Item : SavableObject
{
    public string name;
    public string type; 
    public Stats itemStats;

    public override void loadObject(bool willLoad = true)
    {
        Debug.Log(name);
        SaveManager manager = new SaveManager();
        string path = Application.dataPath + "/itemJson/itemStatsJson/" + name + "Stats.json";
        itemStats = manager.load<Stats>(path);
    }
}
