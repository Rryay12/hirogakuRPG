using JetBrains.Annotations;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.InputSystem;

public class testRunScript : MonoBehaviour
{
    public CharacterInventory exampleCharacter = new CharacterInventory();
    public characterDeckInventory deckInventory = new characterDeckInventory();
    public characterDeck myDeck = new characterDeck();
    public characterDeck enemyDeck = new characterDeck();
    public GameObject battleLogicObject;
    battleLogic exampleBattleLogic;
    void Start()
    {
        exampleCharacter.makeNewCharacter("Reito");
        exampleCharacter.makeNewCharacter("Reito");
        
        myDeck.addCharacterToDeck(0);
        enemyDeck.addCharacterToDeck(1);
        Debug.Log(myDeck.deckSlots[0]);
        deckInventory.saveSelfDeck(myDeck);
        exampleBattleLogic = battleLogicObject.GetComponent<battleLogic>();

        exampleBattleLogic.initializeBattle(myDeck, enemyDeck);
    }

    void Update()
    {

    }
}
