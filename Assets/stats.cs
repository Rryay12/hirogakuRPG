using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


public class Stats : SavableObject
{
    public double maxHp;
    public double maxMana;
    public double phyAttack;
    public double phyDefence;
    public double magAttack;
    public double magDefence;
    public double charisma;
    public double speed;

    public void randomizeStats(double randomMultiplier)
    {
        maxHp += (UnityEngine.Random.value - 0.5) * randomMultiplier;
        maxMana += (UnityEngine.Random.value - 0.5) * randomMultiplier;
        phyAttack += (UnityEngine.Random.value - 0.5) * randomMultiplier;
        phyDefence += (UnityEngine.Random.value - 0.5) * randomMultiplier;
        magAttack += (UnityEngine.Random.value - 0.5) * randomMultiplier;
        magDefence += (UnityEngine.Random.value - 0.5) * randomMultiplier;
        charisma += (UnityEngine.Random.value - 0.5) * randomMultiplier;
        speed += (UnityEngine.Random.value - 0.5) * randomMultiplier;
    }


    public void changeStats(Stats statsChange)
    {
        maxHp = statsChange.maxHp;
        maxMana = statsChange.maxMana;
        phyAttack = statsChange.phyAttack;
        phyDefence = statsChange.phyDefence;
        magAttack = statsChange.magAttack;
        magDefence = statsChange.magDefence;
        charisma = statsChange.charisma;
        speed = statsChange.speed;
    }
    public void addStats(Stats statsChange)
    {
        maxHp += statsChange.maxHp;
        maxMana += statsChange.maxMana;
        phyAttack += statsChange.phyAttack;
        phyDefence += statsChange.phyDefence;
        magAttack += statsChange.magAttack;
        magDefence += statsChange.magDefence;
        charisma += statsChange.charisma;
        speed += statsChange.speed;
    }

    public void multStats(Stats statsChange)
    {
        maxHp = statsChange.maxHp;
        maxMana = statsChange.maxMana;
        phyAttack = statsChange.phyAttack;
        phyDefence = statsChange.phyDefence;
        magAttack = statsChange.magAttack;
        magDefence = statsChange.magDefence;
        charisma = statsChange.charisma;
        speed = statsChange.speed;
    }
}

