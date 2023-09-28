using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SkillTreeSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISaveManager
{
    [SerializeField] string skillName;
    [TextArea]
    [SerializeField] string skillDescription;
    [SerializeField] int skillCost;
    [SerializeField] Color lockedSkillColor;

    [Space]
    [SerializeField] UI_SkillTreeSlot[] shouldBeUnlocked;
    [SerializeField] UI_SkillTreeSlot[] shouldBeLocked;

    public bool unlocked;

    int currency;

    Image skillImage;
    UI ui;

    void OnValidate()
    {
        gameObject.name = "SkillTreeSlot_UI - " + skillName;
    }

    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => UnlockSkillSlot());
    }

    void Start()
    {
        ui = GetComponentInParent<UI>();

        skillImage = GetComponent<Image>();
        skillImage.color = lockedSkillColor;

        if (unlocked)
            skillImage.color = Color.white;
    }

    void Update()
    {
        currency = PlayerManager.instance.GetCurrency();
    }

    public void UnlockSkillSlot()
    {
        if (PlayerManager.instance.HaveEnoughMoney(skillCost) == false) { return; }

        for (int i = 0; i < shouldBeUnlocked.Length; i++)
        {
            if (shouldBeUnlocked[i].unlocked == false)
            {
                Debug.Log("Cannot unlock skill");
                return;
            }
        }

        for (int i = 0; i < shouldBeLocked.Length; i++)
        {
            if (shouldBeLocked[i].unlocked == true)
            {
                Debug.Log("Cannot unlock skill");
                return;
            }
        }

        unlocked = true;
        skillImage.color = Color.white;
    }

    public void OnPointerEnter(PointerEventData eventData) => ui.skillToolTip.ShowToolTip(skillDescription, skillName, skillCost, currency);

    public void OnPointerExit(PointerEventData eventData) => ui.skillToolTip.HideToolTip();

    public void LoadData(GameData _data)
    {
        if (_data.skillTree.TryGetValue(skillName, out bool value))
            unlocked = value;
    }

    public void SaveData(ref GameData _data)
    {
        if (_data.skillTree.TryGetValue(skillName, out bool value))
        {
            _data.skillTree.Remove(skillName);
            _data.skillTree.Add(skillName, unlocked);
        }
        else
            _data.skillTree.Add(skillName, unlocked);
    }
}
