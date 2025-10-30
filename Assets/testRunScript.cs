using JetBrains.Annotations;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.InputSystem;

public class testRunScript : MonoBehaviour
{
    public CharacterInventory exampleCharacter = new CharacterInventory();
    public characterDeck myDeck = new characterDeck();
    public characterDeck enemyDeck = new characterDeck();
    public GameObject battleLogicObject;
    battleLogic exampleBattleLogic;
    void Start()
    {
        myDeck.addCharacterToDeck(0);
        enemyDeck.addCharacterToDeck(1);
        exampleBattleLogic = battleLogicObject.GetComponent<battleLogic>();

        exampleBattleLogic.initializeBattle(myDeck, enemyDeck);
    }

    void Update()
    {

    }
}
