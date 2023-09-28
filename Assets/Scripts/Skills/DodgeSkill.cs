using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class DodgeSkill : Skill
{
    [Header("Dodge")]
    [SerializeField] UI_SkillTreeSlot unlockDodgeButton;
    [SerializeField] int evasionAmount;
    public bool dodgeUnlocked { get; private set; }

    [Header("Mirage dodge")]
    [SerializeField] UI_SkillTreeSlot unlockMirageDodgeButton;
    public bool dodgeMirageUnlocked { get; private set; }

    protected override void Start()
    {
        base.Start();

        unlockDodgeButton.GetComponent<Button>().onClick.AddListener(UnlockDodge);
        unlockMirageDodgeButton.GetComponent<Button>().onClick.AddListener(UnlockedMirageDodge);
    }

    protected override void CheckUnlock()
    {
        Invoke(nameof(UnlockDodge), 1f);
        Invoke(nameof(UnlockedMirageDodge), 1.1f);
    }

    void UnlockDodge()
    {
        if (unlockDodgeButton.unlocked && !dodgeUnlocked)
        {
            player.stats.evasion.AddModifier(evasionAmount);
            Inventory.instance.UpdateStatsUI();
            dodgeUnlocked = true;
        }
    }

    void UnlockedMirageDodge()
    {
        if (unlockMirageDodgeButton.unlocked)
            dodgeMirageUnlocked = true;
    }

    public void CreateMirageOnDodge()
    {
        if (dodgeMirageUnlocked)
            SkillManager.instance.clone.CreateClone(player.transform, new Vector2(2 * player.facingDir,0f));
    }
}
