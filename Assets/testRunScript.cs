using JetBrains.Annotations;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.InputSystem;

public class testRunScript : MonoBehaviour
{
    public CharacterInventory exampleCharacter = new CharacterInventory();
    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            exampleCharacter.makeNewCharacter("Reito");
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Character character = exampleCharacter.getCharacter(0);
            if (character != null)
            {
                Debug.Log("Character ID: " + character.id);
                Debug.Log("Character Name: " + character.name);
                Debug.Log("Character HP: " + character.Hp);
                Debug.Log("Character Mana: " + character.mana);
                Debug.Log("Character Level: " + character.level);
                Debug.Log("Character Moves: " + character.moveNames[0]);
                Debug.Log("Character Moves: " + character.moves[0].name);
            }
            else
            {
                Debug.LogWarning("Character not found.");
            }
        }
    }
}
