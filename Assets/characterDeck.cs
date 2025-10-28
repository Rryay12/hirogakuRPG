using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

public class characterDeck
{
    int[] deckSlots = new int[6];
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