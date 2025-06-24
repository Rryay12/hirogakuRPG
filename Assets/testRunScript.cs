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
            
        }
    }
}
