using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MoveInventory
{
    public SaveManager manager = new SaveManager();
    public Move getMove(string name)
    {
        string path = Application.dataPath + "/moveJson/" + name + ".json";
        Debug.Log(path);
        Move newMove = manager.load<Move>(path);
        return newMove;
    }
}
public class Move : SavableObject
{
    public string name;
    public bool isHeal;
    public Stats moveStats;
    public double phyDamage;
    public double magDamage;

    public override void loadObject(bool willLoad = true)
    {
        Debug.Log(name);
        SaveManager manager = new SaveManager();
        string path = Application.dataPath + "/moveJson/moveStatsJson/" + name + "Stats.json";
        moveStats = manager.load<Stats>(path);
    }
}