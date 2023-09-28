using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HealthBar : MonoBehaviour
{
    Entity entity => GetComponentInParent<Entity>();
    CharacterStats myStats => GetComponentInParent<CharacterStats>();
    RectTransform rectTransform;
    Slider slider;

    void OnEnable()
    {
        entity.onFlipped += FlipUI;
        myStats.onHealthChanged += UpdateHealthBarUI;
    }

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        slider = GetComponentInChildren<Slider>();

        UpdateHealthBarUI();
    }

    void UpdateHealthBarUI()
    {
        slider.maxValue = myStats.GetMaxHealthValue();
        slider.value = myStats.currentHealth;
    }

    void FlipUI() => rectTransform.Rotate(0, 180, 0);

    void OnDisable()
    {
        if (entity != null)
            entity.onFlipped -= FlipUI;

        if (myStats != null)
            myStats.onHealthChanged -= UpdateHealthBarUI;
    }
}
