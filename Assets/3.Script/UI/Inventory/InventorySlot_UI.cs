using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using JetBrains.Annotations;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// �� UI���Կ� ����ִ� ��ũ��Ʈ Sprite�� text ���� item Data�� ����ִ�.
/// </summary>
public class InventorySlot_UI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image itemSprite;
    [SerializeField] private TextMeshProUGUI itemCount;
    [SerializeField] private InventorySlot assignedInventorySlot;

    private Button button;

    public InventorySlot AssignedInventorySlot => assignedInventorySlot;
    public DynamicInventoryDisplay parentDisplay {  get; private set; }
    //�θ� display�ϴ� ������Ʈ ����
    private void Awake()
    {
        ClearSlot();
        button = GetComponent<Button>();
        button?.onClick.AddListener(OnUISlotLeftClick);

        parentDisplay = GetComponentInParent<DynamicInventoryDisplay>();
    }

    public void Init(InventorySlot slot)
    {// �θ� ���� �ʱ�ȭ ����
        assignedInventorySlot = slot;
        UpdateUISlot(slot);
    }

    public void UpdateUISlot(InventorySlot slot)
    {// Ui slot�� ���� inventory itemData�κ��� ����
        if (slot.ItemData != null)
        {
            itemSprite.sprite = assignedInventorySlot.ItemData.Icon;
            itemSprite.color = Color.white;

            if (slot.StackSize > 1) itemCount.text = slot.StackSize.ToString();
            else itemCount.text = string.Empty;
        }

        else
            ClearSlot();
    }

    //public void UpdateUISlot() �ʿ並 �� ��������...
    //{
    //    if(assignedInventorySlot != null) UpdateUISlot(assignedInventorySlot);
    //}

    /// <summary>
    /// ���� slot backend �����͸� clear�ϸ鼭 sprite, text �ʱ�ȭ
    /// </summary>
    public void ClearSlot()
    {
        assignedInventorySlot?.ClearSlot();

        itemSprite.sprite = null;
        itemSprite.color = Color.clear;
        itemCount.text = string.Empty;
    }

    /// <summary>
    /// slot UI�� Ŭ���� ��
    /// </summary>
    public void OnUISlotLeftClick()
    {
        //Access display class
        Debug.Log($"{gameObject} ��Ŭ�� ����");
        parentDisplay?.SlotLeftClicked(this);
    }

    public void OnUISlotRightClick()
    {
        //Access display class
        Debug.Log($"{gameObject} ��Ŭ�� ����");
        parentDisplay?.SlotRightClicked(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // ��Ŭ�� ����
        if(eventData.button.Equals(PointerEventData.InputButton.Right))
        OnUISlotRightClick();
    }
}

