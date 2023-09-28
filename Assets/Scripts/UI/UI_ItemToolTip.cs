using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_ItemToolTip : UI_Tooltip
{
    [SerializeField] TextMeshProUGUI itemNameText;
    [SerializeField] TextMeshProUGUI itemTypeText;
    [SerializeField] TextMeshProUGUI itemDescription;

    public void ShowToolTip(ItemDataEquipment _item)
    {
        if (_item == null) { return; }

        itemNameText.text = _item.itemName;
        itemTypeText.text = _item.equipmentType.ToString();
        itemDescription.text = _item.GetDescription();

        AdjustPosition();

        gameObject.SetActive(true);
    }

    public void HideToolTip() => gameObject.SetActive(false);

}
