using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.Events;

public class GameManager : MonoBehaviour, IInitManager
{
    public Stack<FadeInOut_UI> UIGroupStack = new Stack<FadeInOut_UI>();

    //Obj
    public CanvasGroup itemCanvas;
    public PlayerController playerController;
    public PlayerInput playerInput;
    public PlayerHud_UI PlayerHud_UI;
    public InputActionAsset UIInputActions;

    //Action
    public UnityAction OnResetGame;

    public void Init()
    {

    }

    public void OnClickExit_UIGroup(InputAction.CallbackContext context)
    {
        if (UIGroupStack.Count > 0)
        {
            UIGroupStack.Pop()?.OnClickFade_Btn();

            if (playerInput != null &&
                UIGroupStack.Count == 0)
            {
                Managers.Instance.Game.playerInput.enabled = true;
                Debug.Log("Ŀ�� ����");
                CursorLock(true);
            }
        }
    }

    /// <summary>
    /// �÷��̾ �׾��� ��, ������ ��
    /// �� �� �����ֱ�
    /// </summary>
    public void ResetGame()
    {
        Debug.Log("���� �ʱ�ȭ ���� �߰� �ʿ�");
        OnResetGame?.Invoke();
    }

    public void CursorLock(bool isLock)
    {
        if (isLock)
          Cursor.lockState = CursorLockMode.Locked;
        
        else
            Cursor.lockState = CursorLockMode.None;
        
        
            Cursor.visible = !isLock;
    }
}
