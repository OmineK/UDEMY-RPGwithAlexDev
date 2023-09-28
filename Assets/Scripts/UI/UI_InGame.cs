using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] PlayerStats playerStats;
    [Space]
    [Header("Skill Cooldown images")]
    [SerializeField] Image dashCooldownImage;
    [SerializeField] Image parryCooldownImage;
    [SerializeField] Image crystalCooldownImage;
    [SerializeField] Image swordThrowCooldownImage;
    [SerializeField] Image blackHoleCooldownImage;
    [SerializeField] Image flaskCooldownImage;

    [Header("Skill images")]
    [SerializeField] Image dashImage;
    [SerializeField] Image parryImage;
    [SerializeField] Image crystalImage;
    [SerializeField] Image swordThrowImage;
    [SerializeField] Image blackHoleImage;
    [SerializeField] Image flaskImage;
    [Space]

    [Header("Souls info")]
    [SerializeField] float increaseRate = 400;
    [SerializeField] TextMeshProUGUI currentSkillPoints;
    float soulsAmount;
    [Space]

    SkillManager skill;
    Color defaultFlaskColor;

    void Start()
    {
        if (playerStats != null)
            playerStats.onHealthChanged += UpdateHealthUI;

        skill = SkillManager.instance;

        defaultFlaskColor = flaskImage.color;
    }

    void Update()
    {
        UpdateSoulsUI();
        SkillIconColor();

        if (skill.dash.cooldownTimer > 0)
            SetCooldownOf(dashCooldownImage);

        if (skill.parry.cooldownTimer > 0)
            SetCooldownOf(parryCooldownImage);

        if (skill.crystal.cooldownTimer > 0)
            SetCooldownOf(crystalCooldownImage);

        if (skill.swordThrow.cooldownTimer > 0)
            SetCooldownOf(swordThrowCooldownImage);

        if (skill.blackHole.cooldownTimer > 0)
            SetCooldownOf(blackHoleCooldownImage);

        if (Input.GetKeyDown(KeyCode.Alpha1) && (Inventory.instance.GetEquipment(EquipmentType.Flask) != null))
            SetCooldownOf(flaskCooldownImage);

        CheckCooldownOf(dashCooldownImage, skill.dash.cooldown);
        CheckCooldownOf(parryCooldownImage, skill.parry.cooldown);
        CheckCooldownOf(crystalCooldownImage, skill.crystal.cooldown);
        CheckCooldownOf(swordThrowCooldownImage, skill.swordThrow.cooldown);
        CheckCooldownOf(blackHoleCooldownImage, skill.blackHole.cooldown);

        CheckCooldownOf(flaskCooldownImage, Inventory.instance.flaskCooldown);
    }

    void SkillIconColor()
    {
        if (skill.dash.dashUnlocked)
            dashImage.color = Color.white;

        if (skill.parry.parryUnlocked)
            parryImage.color = Color.white;

        if (skill.crystal.crystalUnlocked)
            crystalImage.color = Color.white;

        if (skill.swordThrow.swordThrowUnlocked)
            swordThrowImage.color = Color.white;

        if (skill.blackHole.blackHoleUnlocked)
            blackHoleImage.color = Color.white;

        if (Inventory.instance.GetEquipment(EquipmentType.Flask) != null)
            flaskImage.color = Color.white;
        else
            flaskImage.color = defaultFlaskColor;
    }

    void UpdateSoulsUI()
    {
        if (soulsAmount < PlayerManager.instance.GetCurrency())
            soulsAmount += Time.deltaTime * increaseRate;
        else
            soulsAmount = PlayerManager.instance.GetCurrency();

        currentSkillPoints.text = ((int)soulsAmount).ToString();
    }

    void UpdateHealthUI()
    {
        slider.maxValue = playerStats.GetMaxHealthValue();
        slider.value = playerStats.currentHealth;
    }

    void SetCooldownOf(Image _image)
    {
        if (_image.fillAmount <= 0)
            _image.fillAmount = 1;
    }

    void CheckCooldownOf(Image _image, float _cooldown)
    {
        if (_image.fillAmount > 0)
            _image.fillAmount -= 1 / _cooldown * Time.deltaTime;
    }
}
