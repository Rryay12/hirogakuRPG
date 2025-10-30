using UnityEngine;

public class battleUI: MonoBehaviour
{
    battleLogic battle;
    GameObject logic;
    
    void Start()
    {
       logic = GameObject.Find("GameLogic");
       battle = logic.GetComponent<battleLogic>();
    }

    public void onMoveButton(int move)
    {
        battle.onMove(move);
    }
}
