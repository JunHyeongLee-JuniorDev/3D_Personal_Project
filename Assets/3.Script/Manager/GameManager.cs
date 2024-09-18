using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class GameManager : MonoBehaviour, IInitManager
{
    public Stack<GameObject> UIGroups = new Stack<GameObject>();

    //Obj
    public CanvasGroup itemCanvas;
    public PlayerInput player;
    public PlayerHud_UI PlayerHud_UI;

    //Events
    public Event asd;

    public void Init()
    {

    }

    public void OnClickExit_UIGroup(InputAction.CallbackContext context)
    {
        if(UIGroups.Count > 0)
        UIGroups.Pop().SetActive(false);
    }

}
