using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Tooltip : MonoBehaviour
{
    [SerializeField] float xLimit = 760;
    [SerializeField] float yLimit = 450;

    [SerializeField] float xOffset = 150;
    [SerializeField] float yOffset = 150;

    public virtual void AdjustPosition()
    {
        Vector2 mousePos = Input.mousePosition;

        float newXoffset = 0;
        float newYoffset = 0;

        if (mousePos.x > xLimit)
            newXoffset = -xOffset;
        else
            newXoffset = xOffset;

        if (mousePos.y > yLimit)
            newYoffset = -yOffset;
        else
            newYoffset = yOffset;

        transform.position = new Vector2(mousePos.x + newXoffset, mousePos.y + newYoffset);
    }

    public void AdjustFontSize(TextMeshProUGUI _text)
    {
        if (_text.text.Length > 12)
            _text.fontSize = _text.fontSize * 0.8f;
    }
}
