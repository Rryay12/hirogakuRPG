using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

[System.Serializable]
public class battleLogic: MonoBehaviour
{
    CharacterInventory playerInventory = new CharacterInventory();
    Character[] playerCharactersInDeck;
    Character[] enemyCharactersInDeck;
    Character playerCharacter;
    Character enemyCharacter;
    Button moveButton0;
    Button moveButton1;
    Button moveButton2;
    Button moveButton3;

    int turnCount = 0;
    int actualTurn = 1; //1 for player, -1 for enemy


    //initiate the battle logic with two characters
    public void initializeBattle(characterDeck playerDeck, characterDeck enemyDeck)
    {
        playerCharactersInDeck = initializeCharacterDeck(playerDeck);
        enemyCharactersInDeck = initializeCharacterDeck(enemyDeck);
        playerCharacter = playerCharactersInDeck[0];
        enemyCharacter = enemyCharactersInDeck[0];
    }

    public void initializeUI()
    {
        initializeButtonText(moveButton0, 0);
        initializeButtonText(moveButton1, 1);
        initializeButtonText(moveButton2, 2);
        initializeButtonText(moveButton3, 3);
    }

    public void initializeButtonText(Button button, int id)
    {
        Move tmpMove = playerCharacter.moves[id];
        if (tmpMove != null)
        {
            button.GetComponentInChildren<TextMeshProUGUI>().text = tmpMove.name;
        }
        else
        {
            button.gameObject.SetActive(false);   
        }
    }
    
    public void startBattle()
    {
        //start battle logic here
        playerCharacter.reinitializeCharacter();
        enemyCharacter.reinitializeCharacter();
        if(playerCharacter.battle_charstats.charisma > enemyCharacter.battle_charstats.charisma)
        {
            actualTurn = 1; //player starts
        }
        else
        {
            actualTurn = -1; //enemy starts
        }
    }

    public void playerturn(string action, Move move = null, Item item = null, int replacementCharId = -1)
    {
        if (action == "run")
        {
            bool escaped = escapeCalculation();
            if (escaped)
            {
                endBattle();
            }
        }

        else if (action == "move" && move != null)
        {
            makeMove(playerCharacter, enemyCharacter, move);
        }
        else if (action == "item" && item != null)
        {

        }
        else if (action == "replace" && replacementCharId != -1)
        {
            //replace character logic here
        }
    }
    public void onRun()
    {
        playerturn("run");
    }

    public void onItem()
    {
        playerturn("item");
    }

    public void replace(int charId)
    {
        playerturn("replace",replacementCharId: charId);
    }

    public void onMove(int moveID)
    {
        playerturn("move", move: playerCharacter.moves[moveID]);
    }

    public bool escapeCalculation()
    {
        float no = Random.value;
        if (no < 0.3f)
        {
            return true;
        }
        return false;
    }
    

    public void BattleLoop()
    {
        //battle loop logic here
    }
    public void endBattle()
    {
        //end battle logic here
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


