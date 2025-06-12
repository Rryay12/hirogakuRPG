using UnityEngine;

[System.Serializable]
public class HpBoost
{
    public int maxHP;
    public int Hp;
}

[System.Serializable]
public class ManaBoost
{
    public int maxMana;
    public int Mana;
}

[System.Serializable]
public class AttackBoost
{
    public int phyAttack;
    public int magAttack;
}

[System.Serializable]
public class DefenceBoost
{
    public int phyDefence;
    public int magDefence;
}

[System.Serializable]
public class Boost
{
    public HpBoost Hp;
    public ManaBoost Mana;
    public AttackBoost Attack;
    public DefenceBoost Defence;
    public int charisma;
    public int spd;
}

[System.Serializable]
public class ItemData
{
    public string itemId;
    public string itemName;
    public string iconPath;
    public string description;
    public string itemType;
    public bool isConsumable;
    public bool isEquippable;
    public bool isStackable;
    public float dropChance;
    public string rarity;
    public Boost boost;

    [System.NonSerialized]
    public Sprite icon;
}