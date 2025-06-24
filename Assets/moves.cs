using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Move : SavableObject
{
    public string name;
    public bool isHeal;
    public Stats moveStats;
    public double phyDamage;
    public double magDamage;
    public Move()
    {
        moveStats = new Stats();
    }
    
}