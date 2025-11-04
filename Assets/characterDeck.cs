using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

public class characterDeckInventory 
{
    public characterDeck deck = new characterDeck();
    string selfPath = Application.dataPath + "/characterDeck/selfCharacterDeck.json";
    string enemyPath = Application.dataPath + "/characterDeck/enemyDeck/";

    SaveManager manager = new SaveManager();
    public characterDeck loadSelfDeck()
    {
        characterDeck loadedDeck = manager.load<characterDeck>(selfPath);
        return loadedDeck;
    }

    public characterDeck loadEnemyDeck(string enemyName)
    {
        characterDeck loadedDeck = manager.load<characterDeck>(enemyPath + enemyName + "Deck.json");
        return loadedDeck;
    }
    public void saveSelfDeck(characterDeck deck)
    {
        manager.save(deck, selfPath);
    }

    public void loadEnemyDeck(characterDeck deck, string enemyName)
    {
        manager.save(deck, enemyPath + enemyName + "Deck.json");
    }
}
public class characterDeck:SavableObject
{
    public int[] deckSlots = new int[6]{-1,-1,-1,-1,-1,-1};
    public bool isEmptySlots()
    {
        for (int i = 0; i < deckSlots.Length; i++)
        {
            if (deckSlots[i] == -1)
            {
                return true;
            }
        }
        return false;
    }

    public void addCharacterToDeck(int charId)
    {
        for (int i = 0; i < deckSlots.Length; i++)
        {
            if (deckSlots[i] == -1)
            {
                deckSlots[i] = charId;
                return;
            }
        }
    }

    public void removeCharacterFromDeck(int charId)
    {
        for (int i = 0; i < deckSlots.Length; i++)
        {
            if (deckSlots[i] == charId)
            {
                deckSlots[i] = -1;
            }
        }
    }

    public int returnCharIdAtSlot(int slotIndex)
    {
        return deckSlots[slotIndex];
    }

}