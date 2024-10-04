using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

/// <summary>
/// 실제 아이템 인스턴스에 부착하는 컴포넌트 콜라이더에 플레이어 들어오면 아이템 추가 판별
/// </summary>
[RequireComponent(typeof(SphereCollider))]
public class ItemPickUp : MonoBehaviour
{
    [SerializeField]
    private string assignName;
    public float pickUpRadius = 1f; // Pick Up 반경
    public InventorySlot itemSlot;
    private InGamePopUp popUp;

    private SphereCollider myCollider;

    private void Start()
    {
        myCollider = GetComponent<SphereCollider>();
        myCollider.isTrigger = true;
        myCollider.radius = pickUpRadius;

        if (itemSlot.StackSize < 0)
        {
            itemSlot = new InventorySlot(Managers.Instance.Data.itemDataBase.GetItemData(assignName),1);
        }

        popUp = Managers.Instance.Game.itemCanvas;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return; // 플레이어가 아니라면 return

        Managers.Instance.Game.itemCanvas.gameObject.SetActive(true);

        popUp.ChangeIcon(Managers.Instance.Data.itemDataBase.GetSprite(itemSlot.Data.displayName));
        popUp.ChangeText("줍기");

        Managers.Instance.Game.playerInput.actions["Interaction"].started -= PickUp;
        Managers.Instance.Game.playerInput.actions["Interaction"].started += PickUp;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return; // 플레이어가 아니라면 return
        popUp.gameObject.SetActive(false);
        Managers.Instance.Game.playerInput.actions["Interaction"].started -= PickUp;
    }

    private void PickUp(InputAction.CallbackContext context)
    {
        switch (itemSlot.Data.itemType)
        {
            case EItemType.GEAR:
                if (Managers.Instance.Inventory.InvSys_Gear.AddToInventory(itemSlot.Data, itemSlot.StackSize))// 아이템이 들어갈 자리가 있다면
                {
                    popUp.gameObject.SetActive(false);
                    Managers.Instance.Game.playerInput.actions["Interaction"].started -= PickUp;
                    Managers.Instance.Inventory.OnDynamicInventoryChanged?.Invoke();
                    gameObject.SetActive(false);// 들어갔다면 false
                }
                break;

            case EItemType.CONSUME: 
                if (Managers.Instance.Inventory.InvSys_Consume.AddToInventory(itemSlot.Data, itemSlot.StackSize))// 아이템이 들어갈 자리가 있다면
                {
                    popUp.gameObject.SetActive(false);
                    Managers.Instance.Game.playerInput.actions["Interaction"].started -= PickUp;
                    Managers.Instance.Inventory.OnDynamicInventoryChanged?.Invoke();
                    gameObject.SetActive(false);// 들어갔다면 false
                }
                break;

            case EItemType.MISSION:
                if (Managers.Instance.Inventory.InvSys_mission.AddToInventory(itemSlot.Data, itemSlot.StackSize))// 아이템이 들어갈 자리가 있다면
                {
                    popUp.gameObject.SetActive(false);
                    Managers.Instance.Game.playerInput.actions["Interaction"].started -= PickUp;
                    Managers.Instance.Inventory.OnDynamicInventoryChanged?.Invoke();
                    gameObject.SetActive(false);// 들어갔다면 false
                }
                break;

            default:
                Debug.LogError("아이템 type이 정해지지 않음");
                break;
        }
    }
}
