using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using System.IO;
using Unity.VisualScripting.ReorderableList;


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
            new_character = new Character(); 
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
        string filepath = Application.dataPath + "/characterJson/charJson/usingChar/" + id + ".json";
        if (File.Exists(filepath) == false)
        {
            Debug.LogWarning("Character file not found: " + filepath);
            return null;
        }
        return manager.load<Character>(filepath);
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
    public Item attackItem;
    public Item defenseItem;
    public string attackItemName;
    public string defenseItemName;
    public double randomMultiplier;
    public bool alive;
    public double XP;
    public int level;
    public string characterImagePath;
    public double elixer;
    public Move[] moves = new Move[4];
    public string[] moveNames;

    public string name;

    public void set_id(int input_id)
    {
        id = input_id;
    }

    public bool isalive()
    {
        return Hp > 0;
    }

    public void initializeCharacter(string name)
    {
        alive = true;
        def_charstats = manager.load<Stats>(Application.dataPath + "/characterJson/charStatsJson/defCharstats/" + name + ".json");
        battle_charstats = def_charstats;
        Hp = def_charstats.maxHp;
        mana = def_charstats.maxMana;
        def_charstats.randomizeStats(randomMultiplier);
        characterImagePath = Application.dataPath + "characterImages/" + name + ".png";
        setmove();
    }

    public void reinitializeCharacter()
    {
        battle_charstats = def_charstats;
        characterImagePath = Application.dataPath + "characterImages/" + name + ".png";
        setmove();
    }

    public void endOfTurn()
    {
        if (Hp <= 0)
        {
            alive = false;
        }

        if (Hp >= battle_charstats.maxHp)
        {
            Hp = battle_charstats.maxHp;
        }

        if (mana > battle_charstats.maxMana)
        {
            mana = battle_charstats.maxMana;
        }

        saveObject();
    }
    
    public void endOfBattle()
    {
        resetStats();
        endOfTurn();
        saveObject();
    }

    public void setmove()
    {
        int count = 0;
        foreach (string moveName in moveNames)
        {
            Debug.Log(moveName);
            Move move = new MoveInventory().getMove(moveName);
            moves[count] = move;
            count++;
        }
    }

    public void equipItem(String item)
    {
        Item newItem = new ItemInventory().getItem(item);
        if (newItem != null)
        {
            if (newItem.isAttackItem)
            {
                attackItem = newItem;
                attackItemName = item;
            }
            else
            {
                defenseItem = newItem;
                defenseItemName = item;
            }
        }
    }
    
    public Move GetMove(string moveName)
    {
        foreach (Move move in moves)
        {
            if (move.name == moveName)
            {
                return move;
            }
        }
        Debug.LogWarning("Move not found: " + moveName);
        return null;
    }


    public override void loadObject(bool willLoad = true)
    {
        if (willLoad)
        {
            def_charstats = manager.load<Stats>(Application.dataPath + "/characterJson/charStatsJson/usingCharstats/" + id.ToString() + ".json");
            battle_charstats = manager.load<Stats>(Application.dataPath + "/characterJson/charStatsJson/battleCharstats/" + id.ToString() + ".json");
            setmove();
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

