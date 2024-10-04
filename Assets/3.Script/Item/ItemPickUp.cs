using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

/// <summary>
/// ���� ������ �ν��Ͻ��� �����ϴ� ������Ʈ �ݶ��̴��� �÷��̾� ������ ������ �߰� �Ǻ�
/// </summary>
[RequireComponent(typeof(SphereCollider))]
public class ItemPickUp : MonoBehaviour
{
    [SerializeField]
    private string assignName;
    public float pickUpRadius = 1f; // Pick Up �ݰ�
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
        if (!other.CompareTag("Player")) return; // �÷��̾ �ƴ϶�� return

        Managers.Instance.Game.itemCanvas.gameObject.SetActive(true);

        popUp.ChangeIcon(Managers.Instance.Data.itemDataBase.GetSprite(itemSlot.Data.displayName));
        popUp.ChangeText("�ݱ�");

        Managers.Instance.Game.playerInput.actions["Interaction"].started -= PickUp;
        Managers.Instance.Game.playerInput.actions["Interaction"].started += PickUp;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return; // �÷��̾ �ƴ϶�� return
        popUp.gameObject.SetActive(false);
        Managers.Instance.Game.playerInput.actions["Interaction"].started -= PickUp;
    }

    private void PickUp(InputAction.CallbackContext context)
    {
        switch (itemSlot.Data.itemType)
        {
            case EItemType.GEAR:
                if (Managers.Instance.Inventory.InvSys_Gear.AddToInventory(itemSlot.Data, itemSlot.StackSize))// �������� �� �ڸ��� �ִٸ�
                {
                    popUp.gameObject.SetActive(false);
                    Managers.Instance.Game.playerInput.actions["Interaction"].started -= PickUp;
                    Managers.Instance.Inventory.OnDynamicInventoryChanged?.Invoke();
                    gameObject.SetActive(false);// ���ٸ� false
                }
                break;

            case EItemType.CONSUME: 
                if (Managers.Instance.Inventory.InvSys_Consume.AddToInventory(itemSlot.Data, itemSlot.StackSize))// �������� �� �ڸ��� �ִٸ�
                {
                    popUp.gameObject.SetActive(false);
                    Managers.Instance.Game.playerInput.actions["Interaction"].started -= PickUp;
                    Managers.Instance.Inventory.OnDynamicInventoryChanged?.Invoke();
                    gameObject.SetActive(false);// ���ٸ� false
                }
                break;

            case EItemType.MISSION:
                if (Managers.Instance.Inventory.InvSys_mission.AddToInventory(itemSlot.Data, itemSlot.StackSize))// �������� �� �ڸ��� �ִٸ�
                {
                    popUp.gameObject.SetActive(false);
                    Managers.Instance.Game.playerInput.actions["Interaction"].started -= PickUp;
                    Managers.Instance.Inventory.OnDynamicInventoryChanged?.Invoke();
                    gameObject.SetActive(false);// ���ٸ� false
                }
                break;

            default:
                Debug.LogError("������ type�� �������� ����");
                break;
        }
    }
}
