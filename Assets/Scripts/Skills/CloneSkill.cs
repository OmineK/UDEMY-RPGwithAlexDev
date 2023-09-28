using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloneSkill : Skill
{
    [Header("Clone info")]
    [SerializeField] GameObject clonePref;
    [SerializeField] float cloneDuration;
    [SerializeField] float attackMultiplier;

    [Header("Clone can attack")]
    [SerializeField] UI_SkillTreeSlot cloneAttackUnlockButton;
    [SerializeField] float cloneAttackMultiplier;
    public bool cloneCanAttackUnlocked { get; private set; }

    [Header("Aggresive clone")]
    [SerializeField] UI_SkillTreeSlot aggresiveCloneUnlockButton;
    [SerializeField] float aggresiveCloneAttackMultiplier;
    public bool canApplyOnHitEffect { get; private set; }

    [Header("Multiple clone")]
    [SerializeField] UI_SkillTreeSlot cloneMultiUnlockButton;
    [SerializeField] float multipleCloneAttackMultiplier;
    [Space]
    [SerializeField][Range(1, 100)] float chanceToDuplicateClone;
    bool canDuplicateClone;

    [Header("Crystal instead of clone")]
    [SerializeField] UI_SkillTreeSlot crystalInsteadUnlockButton;
    public bool crystalInsteadUnlocked { get; private set; }

    protected override void Start()
    {
        base.Start();

        cloneAttackUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneAttack);
        aggresiveCloneUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockAggresiveCloneAttack);
        cloneMultiUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockMultiClone);
        crystalInsteadUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalInsteadOfClone);
    }

    #region Skill unlock region

    protected override void CheckUnlock()
    {
        UnlockCloneAttack();
        UnlockAggresiveCloneAttack();
        UnlockMultiClone();
        UnlockCrystalInsteadOfClone();
    }

    void UnlockCloneAttack()
    {
        if (cloneAttackUnlockButton.unlocked)
        {
            attackMultiplier = cloneAttackMultiplier;
            cloneCanAttackUnlocked = true;
        }
    }

    void UnlockAggresiveCloneAttack()
    {
        if (aggresiveCloneUnlockButton.unlocked)
        {
            attackMultiplier = aggresiveCloneAttackMultiplier;
            canApplyOnHitEffect = true;
        }
    }

    void UnlockMultiClone()
    {
        if (cloneMultiUnlockButton.unlocked)
        {
            attackMultiplier = multipleCloneAttackMultiplier;
            canDuplicateClone = true;
        }
    }

    void UnlockCrystalInsteadOfClone()
    {
        if (crystalInsteadUnlockButton.unlocked)
            crystalInsteadUnlocked = true;
    }

    #endregion

    public void CreateClone(Transform _clonePos, Vector3 _offset)
    {
        if (crystalInsteadUnlocked)
        {
            SkillManager.instance.crystal.CreateCrystal();
            return;
        }

        GameObject newClone = Instantiate(clonePref);

        newClone.GetComponent<CloneController>().
            SetupClone(_clonePos, cloneDuration, cloneCanAttackUnlocked, _offset, FindClosestEnemy(newClone.transform), 
            canDuplicateClone, chanceToDuplicateClone, player, attackMultiplier);
    }

    public void CreateCloneWithDelay(Transform _enemyTransform)
    {
        StartCoroutine(CreateDelayCoroutine(_enemyTransform, new Vector3(2f * player.facingDir, 0f)));
    }

    IEnumerator CreateDelayCoroutine(Transform _transform, Vector3 _offset)
    {
        yield return new WaitForSeconds(0.5f);
        CreateClone(_transform, _offset);
    }
}
