using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParrySkill : Skill
{
    [Header("Parry")]
    [SerializeField] UI_SkillTreeSlot parryUnlockButton;
    public bool parryUnlocked { get; private set; }

    [Header("Parry restore")]
    [SerializeField] UI_SkillTreeSlot parryRestoreUnlockButton;
    [SerializeField][Range(0.1f, 1f)] float restoreHealthPercent;
    public bool parryRestoreUnlocked { get; private set; }

    [Header("Parry with mirage")]
    [SerializeField] UI_SkillTreeSlot parryWithMirageUnlockButton;
    public bool parryWithMirageUnlocked { get; private set; }

    protected override void Start()
    {
        base.Start();

        parryUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParry);
        parryRestoreUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockRestoreParry);
        parryWithMirageUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParryWithMirage);
    }

    protected override void CheckUnlock()
    {
        UnlockParry();
        UnlockRestoreParry();
        UnlockParryWithMirage();
    }

    public override void UseSkill()
    {
        base.UseSkill();

        if (parryRestoreUnlocked)
        {
            int restoreAmount = Mathf.RoundToInt(restoreHealthPercent * player.stats.GetMaxHealthValue());
            player.stats.IncreaseHealthBy(restoreAmount);
        }
    }

    void UnlockParry()
    {
        if (parryUnlockButton.unlocked)
            parryUnlocked = true;
    }

    void UnlockRestoreParry()
    {
        if (parryRestoreUnlockButton.unlocked)
            parryRestoreUnlocked = true;
    }

    void UnlockParryWithMirage()
    {
        if (parryWithMirageUnlockButton.unlocked)
            parryWithMirageUnlocked = true;
    }

    public void MakeMirageOnParry(Transform _respawnTransform)
    {
        if (parryWithMirageUnlocked)
            SkillManager.instance.clone.CreateCloneWithDelay(_respawnTransform);
    }
}
