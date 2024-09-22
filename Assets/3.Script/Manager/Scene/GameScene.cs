using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        sceneType = EScene.GAME;
        Managers.Instance.Inventory.LoadInventory();
        GameObject _itemCanvas = GameObject.Find("ItemCanvas");

        if( _itemCanvas != null )
        Managers.Instance.Game.itemCanvas = _itemCanvas.GetComponent<CanvasGroup>();

        else
            Managers.Instance.Game.itemCanvas = 
                Managers.Instance.InstantiateResouce("Prefabs/UI/ItemCanvas", "ItemCanvas").GetComponent<CanvasGroup>();

        Managers.Instance.Inventory.OnDynamicInventoryChanged?.RemoveAllListeners();
        Managers.Instance.Game.itemCanvas.gameObject.SetActive(false);
        Managers.Instance.Game.playerInput = FindObjectOfType<PlayerInput>();
        Managers.Instance.Game.playerController = Managers.Instance.Game.playerInput.GetComponent<PlayerController>();
        Managers.Instance.Game.PlayerHud_UI = FindObjectOfType<PlayerHud_UI>();
        Managers.Instance.Game.CursorLock(true);
    }

    public override void Clear()
    {
        Managers.Instance.Inventory.OnDynamicInventoryChanged?.RemoveAllListeners();
    }
}
