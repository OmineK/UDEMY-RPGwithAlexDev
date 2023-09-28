using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_SkillToolTip : UI_Tooltip
{
    [SerializeField] float defaultNameFontSize;
    [Space]
    [SerializeField] TextMeshProUGUI skillText;
    [SerializeField] TextMeshProUGUI skillName;
    [SerializeField] TextMeshProUGUI skillCost;
    [SerializeField] TextMeshProUGUI currency;

    void Start()
    {
        HideToolTip();
    }

    public void ShowToolTip(string _skillDescription, string _skillName, int _skillCost, int _currency)
    {
        skillText.text = _skillDescription;
        skillName.text = _skillName;
        skillCost.text = "Cost: " + _skillCost;

        if (_skillCost > _currency)
            currency.color = Color.red;
        else
            currency.color = Color.white;

        currency.text = "Current Souls: " + _currency;

        AdjustPosition();
        gameObject.SetActive(true);
    }

    public void HideToolTip()
    {
        skillName.fontSize = defaultNameFontSize;
        gameObject.SetActive(false);
    }
}
