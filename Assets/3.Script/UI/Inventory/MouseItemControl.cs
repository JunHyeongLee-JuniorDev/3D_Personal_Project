using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MouseItemControl : MonoBehaviour, IPointerExitHandler
{
    private PlayerHud_UI playerHUD_UI;
    public Button first_Btn { get; private set; }
    public Button second_Btn { get; private set; }
    public Button close_Btn { get; private set; }
    public TextMeshProUGUI firstBtn_Text {  get; private set; }
    public TextMeshProUGUI secondBtn_Text { get; private set; }
    public InventorySlot assignedItem { get; private set; }

    private void Awake()
    {
        first_Btn = transform.GetChild(0).GetChild(1).GetComponent<Button>();
        second_Btn = transform.GetChild(0).GetChild(2).GetComponent<Button>();
        close_Btn = transform.GetChild(0).GetChild(3).GetComponent<Button>();
        firstBtn_Text = transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        secondBtn_Text = transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();
        playerHUD_UI = GetComponentInParent<PlayerHud_UI>();
        firstBtn_Text.text = string.Empty;
        secondBtn_Text.text = string.Empty;
        close_Btn.onClick.AddListener(() => {
            PlayClickSound();
            Managers.Instance.Game.UIGroupStack.Pop();
            gameObject.SetActive(false);
            });
    }

    /// <summary>
    /// SetActive = true 로 만들면서 바로 할당해주기
    /// </summary>
    /// <param name="newItemData"></param>
    public void UpdateMouseStat(InventorySlot_UI slot_UI)
    {
        if(slot_UI.AssignedInventorySlot.Data == null) return;// 이거 부르는 곳에서 처리해줘야함
        assignedItem = slot_UI.AssignedInventorySlot;

        first_Btn.onClick.RemoveAllListeners();
        second_Btn.onClick.RemoveAllListeners();

        switch (assignedItem.Data.itemType)
        {
            case EItemType.CONSUME:
                /*
                 * 먹기, 버리기, 닫기
                 */
                second_Btn.gameObject.SetActive(true);
                secondBtn_Text.gameObject.SetActive(true);

                first_Btn.onClick.AddListener(() => 
                {
                    PlayClickSound();
                    assignedItem.RemoveFromStack(1);
                    Managers.Instance.Inventory.InvSys_Consume.OnInventorySlotChanged?.Invoke(assignedItem);
                    OnClickClose();
                    //ㄴ> 효과 발동 로직 추가!
                });

                firstBtn_Text.text = "사용";
                
                second_Btn.onClick.AddListener(() => 
                {
                    PlayClickSound();
                    assignedItem.RemoveFromStack(1);
                    Managers.Instance.Inventory.InvSys_Consume.OnInventorySlotChanged?.Invoke(assignedItem);
                    Managers.Instance.Inventory.OnDynamicInventoryChanged?.Invoke();
                    OnClickClose();
                });

                secondBtn_Text.text = "버리기";
                break;
            case EItemType.GEAR:
                /*
                 * 장착하기, 버리기, 닫기
                 */
                second_Btn.gameObject.SetActive(true);
                secondBtn_Text.gameObject.SetActive(true);

                firstBtn_Text.text = "장착";
                first_Btn.onClick.AddListener(() => 
                {
                    PlayClickSound();
                    Managers.Instance.Inventory.PlayerData.UpdateWeaponAndShield(slot_UI, 1);
                    Managers.Instance.Inventory.PlayerData.OnWeaponChanged?.Invoke();
                    Managers.Instance.Inventory.OnDynamicWeaponChanged?.Invoke();
                    assignedItem.ClearSlot();
                    Managers.Instance.Inventory.InvSys_Gear.OnInventorySlotChanged?.Invoke(slot_UI.AssignedInventorySlot);
                    OnClickClose();
                });

                            
                secondBtn_Text.text = "버리기";
                second_Btn.onClick.AddListener(() => 
                {
                    PlayClickSound();
                    GameObject trash = Instantiate(Resources.Load<GameObject>(Managers.Instance.Data.itemModelPath));
                    trash.GetComponent<ItemPickUp>().itemSlot = new InventorySlot(assignedItem.Data, assignedItem.StackSize);
                    trash.transform.position = Managers.Instance.Game.playerController.transform.position;
                    assignedItem.ClearSlot();
                    Managers.Instance.Inventory.InvSys_Gear.OnInventorySlotChanged?.Invoke(assignedItem);
                    Managers.Instance.Inventory.OnDynamicInventoryChanged?.Invoke();
                    OnClickClose();
                });

                break;
            case EItemType.MISSION:
                /*
                 * 설명, 닫기
                 */
                firstBtn_Text.text = "설명";
                second_Btn.gameObject.SetActive(false);
                secondBtn_Text.gameObject.SetActive(false);

                first_Btn.onClick.AddListener(() => 
                {
                    PlayClickSound();
                    playerHUD_UI.missionDisplay.UpdateUI(assignedItem.Data);
                    OnClickClose();
                });
                break;

            default:
                //뭐노?
                Debug.LogWarning("할당되지 않은 아이템 타입");
                break;
        }

        transform.position = Mouse.current.position.ReadValue();
    }

    private void PlayClickSound()
    {
        Managers.Instance.Sound.Play("UI/MenuClick");
    }

    private void OnDisable()
    {
        assignedItem = null;
        firstBtn_Text.text = string.Empty;
        secondBtn_Text.text = string.Empty;
        first_Btn.onClick.RemoveAllListeners();
        second_Btn.onClick.RemoveAllListeners();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnClickClose();
    }

    private void OnClickClose()
    {
        if (Managers.Instance.Game.UIGroupStack.Count > 0)
        Managers.Instance.Game.UIGroupStack.Pop().OnClickFade_Btn();
        gameObject.SetActive(false);
    }
}
