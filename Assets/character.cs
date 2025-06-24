using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;



[System.Serializable]
public class CharacterInventory
{
    public int current_id;
    public SaveManager manager = new SaveManager();
    public Character new_character;
    public void makeNewCharacter(string name)
    {
        string path = Application.dataPath + "/characterJson/charJson/defaultChar/" + name + ".json";

        Debug.Log(path);
        if (!System.IO.File.Exists(path))
        {
            Debug.LogWarning("Character file not found. Creating new Character.");
            new_character = new Character();  // or load default template
        }
        else
        {
            new_character = manager.load<Character>(path, willDo: false);
        }
        new_character.set_id(current_id);
        new_character.initializeCharacter(name);
        current_id += 1;
        manager.save(new_character, Application.dataPath + "/characterJson/charJson/usingChar/" + new_character.id + ".json");
    }

    public Character getCharacter(int id)
    {
        return manager.load<Character>(Application.dataPath + "/characterJson/charJson/usingChar/" + id + ".json");
    }

}


[System.Serializable]
public class Character : SavableObject
{
    public Stats def_charstats;
    public Stats battle_charstats;
    SaveManager manager = new SaveManager();
    public double id;
    public double Hp;
    public double mana;
    public string item;
    public double randomMultiplier;
    public bool alive;
    public double XP;
    public int level;

    public void set_id(int input_id)
    {
        id = input_id;
    }

    public void initializeCharacter(string name)
    {
        alive = true;
        def_charstats = manager.load<Stats>(Application.dataPath + "/characterJson/charStatsJson/defCharstats/" + name + ".json");
        battle_charstats = def_charstats;
        Hp = def_charstats.maxHp;
        mana = def_charstats.maxMana;
        def_charstats.randomizeStats(randomMultiplier);
    }


    public override void loadObject(bool willLoad = true)
    {
        if (willLoad){
        def_charstats = manager.load<Stats>(Application.dataPath + "/characterJson/charStatsJson/usingCharstats/"+id.ToString()+".json") as Stats;
        battle_charstats = manager.load<Stats>(Application.dataPath + "/characterJson/charStatsJson/battleCharstats/"+id.ToString()+".json") as Stats;
        }
    }

    public override void saveObject(bool willSave = true)
    {
        if (willSave)
        {
        SaveManager manager = new SaveManager();
        manager.save(def_charstats, Application.dataPath + "/characterJson/charStatsJson/usingCharstats/" + id.ToString() + ".json");
        manager.save(battle_charstats, Application.dataPath + "/characterJson/charStatsJson/battleCharstats/" + id.ToString() + ".json");   
        }
    }

    public void resetStats()
    {
        battle_charstats = def_charstats;
    }
}

