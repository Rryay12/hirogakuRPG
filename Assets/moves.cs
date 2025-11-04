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
    public Stats oppMoveStats;
    public Stats selfMoveStats;
    public double phyDamage;
    public double magDamage;
    public double healAmount;
    public double manaCost;
    public double elixerCost;

    public override void loadObject(bool willLoad = true)
    {
        Debug.Log(name);
        SaveManager manager = new SaveManager();
        oppMoveStats = manager.load<Stats>(Application.dataPath + "/moveJson/moveStatsJson/" + name + "/oppMoveStats.json");
        selfMoveStats = manager.load<Stats>(Application.dataPath + "/moveJson/moveStatsJson/" + name + "/selfMoveStats.json");
    }
}