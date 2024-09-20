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
            Managers.Instance.Game.UIGroupStack.Pop();
            gameObject.SetActive(false);
            });
    }

    /// <summary>
    /// SetActive = true 로 만들면서 바로 할당해주기
    /// </summary>
    /// <param name="newItemData"></param>
    public void UpdateMouseStat(InventorySlot newItemData)
    {
        if(newItemData.ItemData == null) return;// 이거 부르는 곳에서 처리해줘야함
        assignedItem = newItemData;

        first_Btn.onClick.RemoveAllListeners();
        second_Btn.onClick.RemoveAllListeners();

        switch (assignedItem.ItemData.ItemType)
        {
            case EItemType.CONSUME:
                /*
                 * 먹기, 버리기, 닫기
                 */
                second_Btn.gameObject.SetActive(true);
                secondBtn_Text.gameObject.SetActive(true);

                first_Btn.onClick.AddListener(() => {
                    assignedItem.RemoveFromStack(1);
                    Managers.Instance.Inventory.InvSys_Consume.OnInventorySlotChanged?.Invoke(assignedItem);
                    OnClickClose();
                    //ㄴ> 효과 발동 로직 추가!
                });

                firstBtn_Text.text = "사용";
                
                second_Btn.onClick.AddListener(() => {
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

                first_Btn.onClick.AddListener(() => {
                    Managers.Instance.Inventory.PlayerData.UseEquipments(newItemData.ItemData, 1);
                    Managers.Instance.Inventory.PlayerData.OnWeaponChanged?.Invoke(newItemData);
                    Managers.Instance.Inventory.OnDynamicWeaponChanged?.Invoke();
                    OnClickClose();
                });

                firstBtn_Text.text = "장착";
                            
                second_Btn.onClick.AddListener(() => {
                    assignedItem.ClearSlot();
                    Managers.Instance.Inventory.PlayerData.OnWeaponChanged?.Invoke(newItemData);
                    Managers.Instance.Inventory.OnDynamicInventoryChanged?.Invoke();
                    if (Managers.Instance.Inventory.PlayerData.equipments["Weapon"]
                    .Equals(assignedItem))
                    {
                        Managers.Instance.Inventory.PlayerData.equipments["Weapon"].ClearSlot();
                        Managers.Instance.Inventory.PlayerData.OnWeaponChanged?
                        .Invoke(Managers.Instance.Inventory.PlayerData.equipments["Weapon"]);
                    }
                    OnClickClose();
                });

                secondBtn_Text.text = "버리기";
                break;
            case EItemType.MISSION:
                /*
                 * 설명, 닫기
                 */
                second_Btn.gameObject.SetActive(false);
                secondBtn_Text.gameObject.SetActive(false);

                first_Btn.onClick.AddListener(() => {
                    playerHUD_UI.missionDisplay.UpdateUI(assignedItem.ItemData);
                    OnClickClose();
                });

                firstBtn_Text.text = "설명";
                break;

            default:
                //뭐노?
                Debug.LogWarning("할당되지 않은 아이템 타입");
                break;
        }

        transform.position = Mouse.current.position.ReadValue();
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
