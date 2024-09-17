using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class GameManager : MonoBehaviour, IInitManager
{
    public Stack<GameObject> UIGroups = new Stack<GameObject>();

    private InputActionAsset inputActions;

    public void Init()
    {
        inputActions = FindObjectOfType<InputSystemUIInputModule>().actionsAsset;

        inputActions["Cancel"].started += OnClickExit_UIGroup;
    }

    public void OnClickExit_UIGroup(InputAction.CallbackContext context)
    {
        if(UIGroups.Count > 0)
        UIGroups.Pop().SetActive(false);
    }
}
