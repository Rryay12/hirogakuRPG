using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class characterCreation : MonoBehaviour
{
    public characterInventory characterInventory = new characterInventory();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            saveInventory();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            loadInventory();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            characterInventory.makeNewCharacter("template");
        }
    }
    public void saveInventory()
    {
        string characterInventoryData = JsonUtility.ToJson(characterInventory);
        string path = Application.dataPath + "/characterInventory.json";
        Debug.Log(path);
        System.IO.File.WriteAllText(path, characterInventoryData);
    }

    public void loadInventory()
    {
        string path = Application.dataPath + "/characterInventory.json";
        string characterInventoryData = System.IO.File.ReadAllText(path);

        characterInventory = JsonUtility.FromJson<characterInventory>(characterInventoryData);
    }
}

[System.Serializable]
public class characterInventory
{
    public List<character> characterList = new List<character>();
    public void makeNewCharacter(string name)
    {
        string path = Application.dataPath + "/characterJson/" + name + ".json";
        string characterData = System.IO.File.ReadAllText(path);

        character new_character = JsonUtility.FromJson<character>(characterData);

        characterList.Add(new_character);
    }
    
    public character getCharacter(int id)
    {
        return characterList.Find(x => x.id == id);
    }

}

[System.Serializable]
public class character
{
    public string name;
    public int id;
    public int Hp;
    public int maxHp;
    public int maxMana;
    public int mana;
    public int phyAttack;
    public int phyDefence;
    public int magAttack;
    public int magDefence;
    public int charisma;
    public int speed;
    public string item;

}