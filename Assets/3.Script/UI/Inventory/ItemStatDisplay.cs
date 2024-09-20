using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemStatDisplay : MonoBehaviour
{
    [field : SerializeField] public Image itemIcon { get; private set; }
    [field : SerializeField] public TextMeshProUGUI itemName_Text { get; private set; }
    [field : SerializeField] public TextMeshProUGUI itemDescription_Text { get; private set; }
    [field : SerializeField] public TextMeshProUGUI itemEffect_Text { get; private set; }
    [field : SerializeField] public TextMeshProUGUI skill_Text { get; private set; }
    [field : SerializeField] public TextMeshProUGUI stat_Text { get; private set; }

    private void Start()
    {
        ClearUI();
    }

    public void ClearUI()
    {
        itemIcon.sprite = null;
        itemIcon.color = Color.clear;
        itemName_Text.text = string.Empty;
        itemDescription_Text.text = string.Empty;
        
        if(itemEffect_Text != null)
            itemEffect_Text.text = string.Empty;

        if(skill_Text != null)
            skill_Text.text = string.Empty;

        if(stat_Text != null)
            stat_Text.text = string.Empty;
    }

    public void UpdateUI(InventoryItemData itemData)
    {
        itemIcon.sprite = itemData.Icon;
        itemIcon.color = Color.white;
        itemName_Text.text = itemData.DisplayName;
        itemDescription_Text.text = itemData.Description;

        if (itemEffect_Text != null)
            itemEffect_Text.text = itemData.EffectString;

        if (skill_Text != null)
            skill_Text.text = (itemData as EquipmentData).Skillstring;

        if (stat_Text != null)
            stat_Text.text = ((int)(itemData as EquipmentData).Stat).ToString();
    }
}
