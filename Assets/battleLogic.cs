using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using System;

[System.Serializable]
public class battleLogic : MonoBehaviour
{
    CharacterInventory playerInventory = new CharacterInventory();
    MoveInventory moveInventory = new MoveInventory();
    Character[] playerCharactersInDeck;
    Character[] enemyCharactersInDeck;
    Character playerCharacter;
    Character enemyCharacter;
    public Button moveButton0;
    public Button moveButton1;
    public Button moveButton2;
    public Button moveButton3;
    bool battleActive = false;

    void Update()
    {
        Debug.Log(battleActive);
        if (battleActive)
        {
            updateStats();
            updateUI();
            playerCharacter.endOfTurn();
            enemyCharacter.endOfTurn();
            Debug.Log("Player Elixer: " + playerCharacter.elixer + " Enemy Hp: " + enemyCharacter.Hp +"is enemy alive"+ enemyCharacter.isalive());
        }
        checkBattleOver();
    }

    public void checkBattleOver()
    {
        if (isAllDead(playerCharactersInDeck))
        {
            Debug.Log("Player has no remaining characters. You lose!");
            endBattle();
        }
        else if (isAllDead(enemyCharactersInDeck))
        {
            Debug.Log("Enemy has no remaining characters. You win!");
            endBattle();
        }
    }
    public void updateStats()
    {

        playerCharacter.elixer = playerCharacter.elixer + playerCharacter.battle_charstats.elixerRegen * Time.deltaTime;
        playerCharacter.mana = playerCharacter.mana + playerCharacter.battle_charstats.manaRegen * Time.deltaTime;
        enemyCharacter.elixer = enemyCharacter.elixer + enemyCharacter.battle_charstats.elixerRegen * Time.deltaTime;
        enemyCharacter.mana = enemyCharacter.mana + enemyCharacter.battle_charstats.manaRegen * Time.deltaTime;

        if (playerCharacter.elixer > playerCharacter.battle_charstats.maxElixer)
        {
            playerCharacter.elixer = playerCharacter.battle_charstats.maxElixer;
        }
        if (enemyCharacter.elixer > enemyCharacter.battle_charstats.maxElixer)
        {
            enemyCharacter.elixer = enemyCharacter.battle_charstats.maxElixer;
        }
        if (playerCharacter.mana > playerCharacter.battle_charstats.maxMana)
        {
            playerCharacter.mana = playerCharacter.battle_charstats.maxMana;
        }
        if (enemyCharacter.mana > enemyCharacter.battle_charstats.maxMana)
        {
            enemyCharacter.mana = enemyCharacter.battle_charstats.maxMana;
        }
    }

    public void updateUI()
    {
        updateButton(moveButton0);
        updateButton(moveButton1);
        updateButton(moveButton2);
        updateButton(moveButton3);
    }
    
    public void updateButton(Button button)
    {
        if( button.gameObject.activeSelf == false)
        {
            return;
        }
        GameObject cooldownTimer = button.transform.Find("elixerTimer").gameObject;
        string movename = button.GetComponentInChildren<TextMeshProUGUI>().text;
        double maxElixerMove = moveInventory.getMove(movename).elixerCost;
        cooldownTimer.GetComponent<Image>().fillAmount = (float)Math.Min(playerCharacter.elixer / maxElixerMove,1f);
    }

    //initiate the battle logic with two characters
    public void initializeBattle(characterDeck playerDeck, characterDeck enemyDeck)
    {
        playerCharactersInDeck = initializeCharacterDeck(playerDeck);
        enemyCharactersInDeck = initializeCharacterDeck(enemyDeck);
        initializeCharacters();
        playerCharacter = playerCharactersInDeck[0];
        enemyCharacter = enemyCharactersInDeck[0];
        initializeUI();
        battleActive = true;
    }

    public void initializeCharacters()
    {
        for (int i = 0; i < playerCharactersInDeck.Length; i++)
        {
            if (playerCharactersInDeck[i] != null)
            {
                playerCharactersInDeck[i].reinitializeCharacter();
            }
        }
        for (int i = 0; i < enemyCharactersInDeck.Length; i++)
        {
            if (enemyCharactersInDeck[i] != null)
            {
                enemyCharactersInDeck[i].reinitializeCharacter();
            }
        }
    }

    public void endCharacters()
    {
        for (int i = 0; i < playerCharactersInDeck.Length; i++)
        {
            if (playerCharactersInDeck[i] != null)
            {
                playerCharactersInDeck[i].endOfBattle();
            }
        }
        for (int i = 0; i < enemyCharactersInDeck.Length; i++)
        {
            if (enemyCharactersInDeck[i] != null)
            {
                enemyCharactersInDeck[i].endOfBattle();
            }
        }
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
            button.gameObject.SetActive(true);
            TextMeshProUGUI text = button.GetComponentInChildren<TextMeshProUGUI>();
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
            makeMove(move, true);
            playerCharacter.endOfTurn();
            enemyCharacter.endOfTurn();
            Debug.Log(playerCharacter.name + " used " + move.name);
            Debug.Log("enemy "+enemyCharacter.name + " has " + enemyCharacter.Hp + " HP left.");
        }
        else if (action == "item" && item != null)
        {

        }
        else if (action == "replace" && replacementCharId != -1)
        {
            playerCharacter.endOfBattle();
            playerCharacter = playerInventory.getCharacter(replacementCharId);;
            playerCharacter.reinitializeCharacter();
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
        playerturn("replace", replacementCharId: charId);
    }

    public void onMove(int moveID)
    {
        playerturn("move", move: playerCharacter.moves[moveID]);
    }


    public bool escapeCalculation()
    {
        float no = UnityEngine.Random.Range(0f, 1f);
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
        battleActive = false;
        Debug.Log("Battle Ended");
        playerCharacter.endOfBattle();
        enemyCharacter.endOfBattle();
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

    public int calculateDamage(Character attacker, Character defender, Move move)
    {
        double phyDamage = attacker.battle_charstats.phyAttack / 100 * move.phyDamage *(1-Math.Tanh(defender.battle_charstats.phyDefence / 500));
        double magDamage = attacker.battle_charstats.magAttack / 100 * move.magDamage *(1-Math.Tanh(defender.battle_charstats.magDefence / 500));
        int damage = Mathf.RoundToInt((float)(magDamage + phyDamage));
        return damage;
    }
    public void makeMove(Move move, bool isPlayerMove)
    {
        if (isPlayerMove)
        {
            if (playerCharacter.elixer < move.elixerCost)
            {
                Debug.Log("Not enough elixer to perform the move!");
                return;
            }
            if(playerCharacter.mana < move.manaCost)
            {
                Debug.Log("Not enough mana to perform the move!");
                return;
            }
            double damage = calculateDamage(playerCharacter, enemyCharacter, move);
            double heal = playerCharacter.battle_charstats.maxHp * move.healAmount;
            damage = Mathf.RoundToInt((float)damage);
            heal = Mathf.RoundToInt((float)heal);
            playerCharacter.battle_charstats.multStats(move.selfMoveStats);
            enemyCharacter.battle_charstats.multStats(move.oppMoveStats);
            enemyCharacter.Hp -= (int)damage;
            playerCharacter.Hp += (int)heal;
            playerCharacter.elixer -= move.elixerCost;
        }
        else
        {
            if (enemyCharacter.elixer < move.elixerCost)
            {
                Debug.Log("Enemy does not have enough elixer to perform the move!");
                return;
            }
            if (enemyCharacter.mana < move.manaCost){
                Debug.Log("Enemy does not have enough mana to perform the move!");
                return;
            }
            double damage = calculateDamage(enemyCharacter, playerCharacter, move);
            double heal = enemyCharacter.battle_charstats.maxHp * move.healAmount;
            damage = Mathf.RoundToInt((float)damage);
            heal = Mathf.RoundToInt((float)heal);
            enemyCharacter.battle_charstats.changeStats(move.selfMoveStats);
            playerCharacter.Hp -= damage;
            enemyCharacter.Hp += (int)heal;
            playerCharacter.battle_charstats.changeStats(move.oppMoveStats);
            enemyCharacter.elixer -= move.elixerCost;
        }

    }
}


