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
    private SphereCollider myCollider;
    private Tween tweenCash;

    private void Start()
    {
        myCollider = GetComponent<SphereCollider>();
        myCollider.isTrigger = true;
        myCollider.radius = pickUpRadius;

        if (itemSlot.StackSize < 0)
        {
            itemSlot = new InventorySlot(Managers.Instance.Data.itemDataBase.GetItemData(assignName),1);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return; // �÷��̾ �ƴ϶�� return

        Managers.Instance.Game.itemCanvas.gameObject.SetActive(true);
        Managers.Instance.Game.itemCanvas.transform.GetChild(0).GetComponent<Image>().sprite = 
            Managers.Instance.Data.itemDataBase.GetSprite(itemSlot.Data.displayName);
        Managers.Instance.Game.playerInput.actions["Interaction"].started += PickUp;

        tweenCash?.Kill();
        tweenCash = Managers.Instance.Game.itemCanvas.DOFade(0.8f, 1.0f).SetLoops(-1,LoopType.Yoyo).SetEase(Ease.Linear);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return; // �÷��̾ �ƴ϶�� return
        CanvasGroup interactionCanvas = Managers.Instance.Game.itemCanvas.GetComponentInChildren<CanvasGroup>();
        tweenCash?.Kill();
        interactionCanvas.alpha = 1.0f;
        interactionCanvas.gameObject.SetActive(false);
        Managers.Instance.Game.playerInput.actions["Interaction"].started -= PickUp;
    }

    private void PickUp(InputAction.CallbackContext context)
    {
        switch (itemSlot.Data.itemType)
        {
            case EItemType.GEAR:
                if (Managers.Instance.Inventory.InvSys_Gear.AddToInventory(itemSlot.Data, itemSlot.StackSize))// �������� �� �ڸ��� �ִٸ�
                {
                    CanvasGroup interactionCanvas = Managers.Instance.Game.itemCanvas.GetComponentInChildren<CanvasGroup>();
                    tweenCash?.Kill();
                    interactionCanvas.alpha = 1.0f;
                    interactionCanvas.gameObject.SetActive(false);
                    Managers.Instance.Game.playerInput.actions["Interaction"].started -= PickUp;
                    Managers.Instance.Inventory.OnDynamicInventoryChanged?.Invoke();
                    Destroy(gameObject); // ���ٸ� Destroy
                }
                break;

            case EItemType.CONSUME: 
                if (Managers.Instance.Inventory.InvSys_Consume.AddToInventory(itemSlot.Data, itemSlot.StackSize))// �������� �� �ڸ��� �ִٸ�
                {
                    CanvasGroup interactionCanvas = Managers.Instance.Game.itemCanvas.GetComponentInChildren<CanvasGroup>();
                    tweenCash?.Kill();
                    interactionCanvas.alpha = 1.0f;
                    interactionCanvas.gameObject.SetActive(false);
                    Managers.Instance.Game.playerInput.actions["Interaction"].started -= PickUp;
                    Managers.Instance.Inventory.OnDynamicInventoryChanged?.Invoke();
                    Destroy(gameObject); // ���ٸ� Destroy
                }
                break;

            case EItemType.MISSION:
                if (Managers.Instance.Inventory.InvSys_mission.AddToInventory(itemSlot.Data, itemSlot.StackSize))// �������� �� �ڸ��� �ִٸ�
                {
                    CanvasGroup interactionCanvas = Managers.Instance.Game.itemCanvas.GetComponentInChildren<CanvasGroup>();
                    tweenCash?.Kill();
                    interactionCanvas.alpha = 1.0f;
                    interactionCanvas.gameObject.SetActive(false);
                    Managers.Instance.Game.playerInput.actions["Interaction"].started -= PickUp;
                    Managers.Instance.Inventory.OnDynamicInventoryChanged?.Invoke();
                    Destroy(gameObject); // ���ٸ� Destroy
                }
                break;

            default:
                Debug.LogError("������ type�� �������� ����");
                break;
        }
    }
}
