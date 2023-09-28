using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UI_StatToolTip : UI_Tooltip
{
    [SerializeField] TextMeshProUGUI statDescription;

    public void ShowStatToolTip(string _text)
    {
        statDescription.text = _text;
        AdjustPosition();
 
        gameObject.SetActive(true);
    }

    public void HideStatToolTip()
    {
        statDescription.text = "";

        gameObject.SetActive(false);
    }
}
