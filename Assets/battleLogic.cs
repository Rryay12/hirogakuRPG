using UnityEngine;

[System.Serializable]
public class battleLogic
{
    CharacterInventory playerInventory = new CharacterInventory();
    Character[] playerCharactersInDeck;
    Character[] enemyCharactersInDeck;
    Character playerCharacer;
    Character enemyCharacter;

    //initiate the battle logic with two characters
    public battleLogic(characterDeck playerDeck, characterDeck enemyDeck)
    {
        playerCharactersInDeck = initializeCharacterDeck(playerDeck);
        enemyCharactersInDeck = initializeCharacterDeck(enemyDeck);
        playerCharacer = playerCharactersInDeck[0];
        enemyCharacter = enemyCharactersInDeck[0];
    }

    public Character[] initializeCharacterDeck(characterDeck deck)
    {
        Character[] charactersInDeck = new Character[6];
        for (int i = 0; i < 6; i++)
        {
            if (deck.returnCharIdAtSlot(i) != -1)
            {
                charactersInDeck[i] = playerInventory.getCharacter(deck.returnCharIdAtSlot(i));
            }
            else
            {
                charactersInDeck[i] = null;
            }
        }
        return charactersInDeck;
    }

    //are they all dead?
    public bool isAllDead(Character[] charactersInDeck)
    {
        bool allDead = true;
        for (int i = 0; i < charactersInDeck.Length; i++)
        {
            if (charactersInDeck[i] != null && charactersInDeck[i].isalive())
            {
                allDead = false;
                break;
            }
        }
        return allDead;
    }

    //make a move from attacker to defender using the specified move
    public void makeMove(Character attacker, Character defender, Move move)
    {
        if (move.isHeal == false)
        {
            double damage = attacker.battle_charstats.phyAttack / 100 * move.moveStats.phyAttack + attacker.battle_charstats.magAttack / 100 * move.moveStats.magAttack;
            damage = Mathf.RoundToInt((float)damage);
            defender.battle_charstats.changeStats(move.moveStats);
            defender.Hp -= damage;
        }
        else
        {
            double heal = attacker.battle_charstats.phyAttack / 100 * move.moveStats.phyAttack + attacker.battle_charstats.magAttack / 100 * move.moveStats.magAttack;
            heal = Mathf.RoundToInt((float)heal);
            attacker.battle_charstats.changeStats(move.moveStats);
            attacker.Hp += heal;
            if (defender.Hp > defender.def_charstats.maxHp) defender.Hp = defender.def_charstats.maxHp;
        }
    }
}


