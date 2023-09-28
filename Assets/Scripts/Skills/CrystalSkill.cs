using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CrystalSkill : Skill
{
    [Header("Simple crystal")]
    [SerializeField] UI_SkillTreeSlot crystalUnlockButton;
    public bool crystalUnlocked { get; private set; }

    [Header("Crystal mirage")]
    [SerializeField] UI_SkillTreeSlot cloneInsteadUnlockButton;
    [SerializeField] bool cloneInsteadOfCrystal;

    [Header("Explosive crystal")]
    [SerializeField] UI_SkillTreeSlot crystalExplodeUnlockButton;
    [SerializeField] bool canExplode;

    [Header("Moving crystal")]
    [SerializeField] UI_SkillTreeSlot crystalMoveUnlockButton;
    [SerializeField] bool canMoveToEnemy;
    [SerializeField] float moveSpeed;

    [Header("Multi stacking crystal")]
    [SerializeField] UI_SkillTreeSlot crystalMultiUnlockButton;
    [SerializeField] bool canUseMultiStacks;
    [SerializeField] float multiStackCooldown;
    [SerializeField] float useTimeWindow;
    [SerializeField] int amountOfStacks;
    [SerializeField] List<GameObject> crystalLeft = new List<GameObject>();

    [Space]
    [SerializeField] float crystalDuration;
    [SerializeField] GameObject crystalPref;

    public GameObject currentCrystal;

    protected override void Start()
    {
        base.Start();

        crystalUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCrystal);
        cloneInsteadUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalMirage);
        crystalExplodeUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockExplosive);
        crystalMoveUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockMovingCrystal);
        crystalMultiUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockMultiCrystal);
    }

    #region Unlock skill region

    protected override void CheckUnlock()
    {
        UnlockCrystal();
        UnlockCrystalMirage();
        UnlockExplosive();
        UnlockMovingCrystal();
        UnlockMultiCrystal();
    }

    void UnlockCrystal()
    {
        if (crystalUnlockButton.unlocked)
            crystalUnlocked = true;
    }

    void UnlockCrystalMirage()
    {
        if (cloneInsteadUnlockButton.unlocked)
            cloneInsteadOfCrystal = true;
    }

    void UnlockExplosive()
    {
        if (crystalExplodeUnlockButton.unlocked)
            canExplode = true;
    }

    void UnlockMovingCrystal()
    {
        if (crystalMoveUnlockButton.unlocked)
            canMoveToEnemy = true;
    }

    void UnlockMultiCrystal()
    {
        if (crystalMultiUnlockButton.unlocked)
            canUseMultiStacks = true;
    }

    #endregion

    public override bool CanUseSkill()
    {
        if (cooldownTimer < 0)
        {
            if (crystalUnlocked && !canMoveToEnemy && (currentCrystal == null))
            {
                CreateCrystal();
                return false;
            }

            UseSkill();
            cooldownTimer = cooldown;
            return true;
        }

        player.fx.CreatePopUpText("Cooldown");

        return false;
    }

    public override void UseSkill()
    {
        base.UseSkill();

        if (CanUseMultiCrystal()) { return; }

        if (currentCrystal == null)
            CreateCrystal();
        else
        {
            if (canMoveToEnemy) { return; }

            Vector2 playerPos = player.transform.position;
            player.transform.position = currentCrystal.transform.position;
            currentCrystal.transform.position = playerPos;

            if (cloneInsteadOfCrystal)
            {
                SkillManager.instance.clone.CreateClone(currentCrystal.transform, Vector3.zero);
                Destroy(currentCrystal);
            }
            else
                currentCrystal.GetComponent<CrystalController>()?.FinishCrystal();
        }
    }

    bool CanUseMultiCrystal()
    {
        if (canUseMultiStacks)
        {
            if (crystalLeft.Count > 0)
            {
                if (crystalLeft.Count == amountOfStacks)
                    Invoke(nameof(ResetAbility), useTimeWindow);

                cooldown = 0;

                GameObject crystalToSpawn = crystalLeft[crystalLeft.Count - 1];
                GameObject newCrystal = Instantiate(crystalToSpawn, player.transform.position, Quaternion.identity);

                crystalLeft.Remove(crystalToSpawn);

                newCrystal.GetComponent<CrystalController>().
                    SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(newCrystal.transform), player);

                if (crystalLeft.Count <= 0)
                {
                    cooldown = multiStackCooldown;
                    RefilCrystal();
                }
            }

            return true;
        }

        return false;
    }

    public void CreateCrystal()
    {
        currentCrystal = Instantiate(crystalPref, player.transform.position, Quaternion.identity);
        CrystalController currentCrystalScript = currentCrystal.GetComponent<CrystalController>();

        currentCrystalScript.SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(currentCrystal.transform), player);
    }

    public void CurrentCrystalChooseRandomTarget() => currentCrystal.GetComponent<CrystalController>().ChooseRandomEnemy();

    void ResetAbility()
    {
        if (cooldownTimer > 0) { return; }

        cooldownTimer = multiStackCooldown;
        RefilCrystal();
    }

    void RefilCrystal()
    {
        int amountToAdd = amountOfStacks - crystalLeft.Count;

        for (int i = 0; i < amountToAdd; i++)
        {
            crystalLeft.Add(crystalPref);
        }
    }
}
