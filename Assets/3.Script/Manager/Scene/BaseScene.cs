using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public abstract class BaseScene : MonoBehaviour
{
    public EScene sceneType { get; protected set; } = EScene.UNKNOWN;

    private void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        EventSystem eventSystem = FindObjectOfType<EventSystem>();

        if (eventSystem == null)
            eventSystem = Managers.Instance.InstantiateResouce("Prefabs/UI/EventSystem", "EventSystem").GetComponent<EventSystem>();

        InputActionAsset inputActions = eventSystem.GetComponent<InputSystemUIInputModule>().actionsAsset;
        Managers.Instance.Game.UIInputActions = inputActions;

        inputActions["Cancel"].canceled -= Managers.Instance.Game.OnClickExit_UIGroup;
        inputActions["Cancel"].canceled += Managers.Instance.Game.OnClickExit_UIGroup;
    }

    public abstract void Clear();
}
