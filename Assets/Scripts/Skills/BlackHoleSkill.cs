using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackHoleSkill : Skill
{
    [SerializeField] UI_SkillTreeSlot blackHoleUnlockButton;
    [SerializeField] int amountOfAtttack;
    [SerializeField] float cloneAttackCooldown;
    [SerializeField] float blackHoleDuration;
    [Space]
    [SerializeField] GameObject blackHolePref;
    [SerializeField] float maxSize;
    [SerializeField] float growSpeed;
    [SerializeField] float shrinkSpeed;
    public bool blackHoleUnlocked { get; private set; }

    BlackHoleController currentBlackHole;

    protected override void Start()
    {
        base.Start();

        blackHoleUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockBlackHole);
    }

    protected override void Update()
    {
        base.Update();
    }

    void UnlockBlackHole()
    {
        if (blackHoleUnlockButton.unlocked)
            blackHoleUnlocked = true;
    }

    protected override void CheckUnlock()
    {
        UnlockBlackHole();
    }

    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();

        AudioManager.instance.PlaySFX(3, null);
        AudioManager.instance.PlaySFX(6, null);

        GameObject newBlackHole = Instantiate(blackHolePref, player.transform.position, Quaternion.identity);
        currentBlackHole = newBlackHole.GetComponent<BlackHoleController>();

        currentBlackHole.SetupBlackHole(maxSize, growSpeed, shrinkSpeed, amountOfAtttack, cloneAttackCooldown, blackHoleDuration);
    }

    public bool SkillCompleted()
    {
        if (!currentBlackHole)
            return false;

        if (currentBlackHole.playerCanExitState)
        {
            currentBlackHole = null;
            return true;
        }

        return false;
    }

    public float GetBlackHoleRadius()
    {
        return maxSize / 2;
    }
}
