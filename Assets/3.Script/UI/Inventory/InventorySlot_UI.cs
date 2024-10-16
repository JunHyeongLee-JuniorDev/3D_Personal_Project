using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using JetBrains.Annotations;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// 각 UI슬롯에 들어있는 스크립트 Sprite와 text 실제 item Data가 들어있다.
/// </summary>
public class InventorySlot_UI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image itemSprite;
    [SerializeField] private TextMeshProUGUI itemCount;
    [SerializeField] private InventorySlot assignedInventorySlot;
    private Button button;

    public InventorySlot AssignedInventorySlot => assignedInventorySlot;
    public DynamicInventoryDisplay parentDisplay {  get; private set; }
    public PlayerSetDisplayer anotherSetDisplay { get; private set; }
    //부모에 display하는 컴포넌트 참조
    private void Awake()
    {
        ClearSlot();
        button = GetComponent<Button>();
        button?.onClick.AddListener(OnUISlotLeftClick);
        parentDisplay = GetComponentInParent<DynamicInventoryDisplay>();
        anotherSetDisplay = GetComponentInParent<PlayerSetDisplayer>();
    }

    public void Init(InventorySlot slot)
    {// 부모 에서 초기화 실행
        assignedInventorySlot = slot;
        UpdateUISlot(slot);
    }

    public void UpdateUISlot(InventorySlot slot)
    {// Ui slot을 실제 inventory itemData로부터 갱신

        if (slot.StackSize > 1 || slot.Data?.itemType == EItemType.CONSUME)
        { 
            itemCount.text = slot.StackSize.ToString();
            itemSprite.sprite = Managers.Instance.Data.itemDataBase.GetSprite(slot.Data.displayName);
            itemSprite.color = Color.white;
        }


        else if (slot.StackSize == 1)
        {
            itemCount.text = string.Empty;
            itemSprite.sprite = Managers.Instance.Data.itemDataBase.GetSprite(slot.Data.displayName);
            itemSprite.color = Color.white;
        }

        else
        {
            slot.ClearSlot();
            ClearSlot();
        }
    }

    /// <summary>
    /// 실제 slot backend 데이터를 clear하면서 sprite, text 초기화
    /// </summary>
    public void ClearSlot()
    {
        assignedInventorySlot = new InventorySlot();

        itemSprite.sprite = null;
        itemSprite.color = Color.clear;
        itemCount.text = string.Empty;
    }

    /// <summary>
    /// slot UI를 클릭할 시
    /// </summary>
    public void OnUISlotLeftClick()
    {
        PlayClickSound();
        parentDisplay?.SlotLeftClicked(this);
        anotherSetDisplay?.OnLeftClicked(this);
    }

    public void OnUISlotRightClick()
    {
        PlayClickSound();
        parentDisplay?.SlotRightClicked(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // 우클릭 구현
        if (eventData.button.Equals(PointerEventData.InputButton.Right))
            OnUISlotRightClick();
    }

    private void PlayClickSound()
    {
        Managers.Instance.Sound.Play("UI/MenuClick");
    }
}

