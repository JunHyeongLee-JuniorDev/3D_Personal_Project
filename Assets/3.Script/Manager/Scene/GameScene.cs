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
        InGamePopUp _itemCanvas = FindObjectOfType<InGamePopUp>();

        if( _itemCanvas != null )
        Managers.Instance.Game.itemCanvas = _itemCanvas;

        else
            Managers.Instance.Game.itemCanvas = 
                Managers.Instance.InstantiateResouce("Prefabs/UI/ItemCanvas", "ItemCanvas").GetComponent<InGamePopUp>();

        Managers.Instance.Inventory.OnDynamicInventoryChanged?.RemoveAllListeners();
        Managers.Instance.Inventory.PlayerData.OnWeaponChanged?.RemoveAllListeners();
        Managers.Instance.Inventory.PlayerData.OnChangeHealPotion?.RemoveAllListeners();
        Managers.Instance.Inventory.PlayerData.OnRefillStatus?.RemoveAllListeners();
        Managers.Instance.Inventory.PlayerData.OnReduceStatus?.RemoveAllListeners();

        Managers.Instance.Game.itemCanvas.gameObject.SetActive(false);
        Managers.Instance.Game.playerController = FindObjectOfType<PlayerController>();

        if (Managers.Instance.Game.playerController == null)
        {
            Managers.Instance.Game.playerController =
            Managers.Instance.InstantiateResouce("Prefabs/Player/Player", "Player").GetComponent<PlayerController>();
            Managers.Instance.Game.playerController.transform.position = Managers.Instance.Inventory.PlayerData.playerPosition;
            Managers.Instance.Game.playerController.transform.rotation = Managers.Instance.Inventory.PlayerData.playerRotation;
        }

        Managers.Instance.Game.playerInput = Managers.Instance.Game.playerController.GetComponent<PlayerInput>();
        Managers.Instance.Game.PlayerHud_UI = FindObjectOfType<PlayerHud_UI>();
        Managers.Instance.Game.CursorLock(true);
        Managers.Instance.Game.CreateDiedPopUp();

        Managers.Instance.Game.playerController.OnPlayerDead -= Managers.Instance.Game.OnDiedPopUp;
        Managers.Instance.Game.playerController.OnPlayerDead += Managers.Instance.Game.OnDiedPopUp;
    }

    public override void Clear()
    {
        Managers.Instance.Inventory.OnDynamicInventoryChanged?.RemoveAllListeners();
    }
}
