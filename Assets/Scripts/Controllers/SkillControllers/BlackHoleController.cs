using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleController : MonoBehaviour
{
    [SerializeField] GameObject hotKeyPref;
    [SerializeField] List<KeyCode> keyCodeList;

    public bool blackHoleIsRunning { get; private set; } = false;
    public bool playerCanExitState { get; private set; }
    public List<Transform> targets = new List<Transform>();

    bool canGrow = true;
    bool canShrink = false;
    bool canCreateHotKeys = true;
    bool cloneAttackReleasted = false;
    bool playerCanDisapear = true;

    float maxSize;
    float growSpeed;
    float shrinkSpeed;
    float cloneAttackTimer;
    float cloneAttackCooldown;
    float blackHoleTimer;

    int amountOfAtttack;

    List<GameObject> createdHotKey = new List<GameObject>();

    public void SetupBlackHole(float _maxSize, float _growSpeed, float _shrinkSpeed,
                               int _ammountOfAttack, float _cloneAttackCooldown, float _blackHoleDuration)
    {
        maxSize = _maxSize;
        growSpeed = _growSpeed;
        shrinkSpeed = _shrinkSpeed;
        amountOfAtttack = _ammountOfAttack;
        cloneAttackCooldown = _cloneAttackCooldown;
        blackHoleTimer = _blackHoleDuration;

        if (SkillManager.instance.clone.crystalInsteadUnlocked)
            playerCanDisapear = false;
    }

    void Update()
    {
        cloneAttackTimer -= Time.deltaTime;
        blackHoleTimer -= Time.deltaTime;

        if (blackHoleTimer < 0)
        {
            blackHoleTimer = Mathf.Infinity;

            if (targets.Count > 0)
                ReleaseCloneAttack();
            else
                FinishBlackHoleAbility();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ReleaseCloneAttack();
        }

        CloneAttackLogic();

        if (canGrow && !canShrink)
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);

        if (canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);

            if (transform.localScale.x < 0)
                Destroy(this.gameObject);
        }
    }

    void ReleaseCloneAttack()
    {
        if (targets.Count <= 0) { return; }

        DestroyHotKeys();
        cloneAttackReleasted = true;
        canCreateHotKeys = false;

        if (playerCanDisapear)
        {
            playerCanDisapear = false;
            PlayerManager.instance.player.fx.MakeTransparent(true);
        }
    }

    void DestroyHotKeys()
    {
        if (createdHotKey.Count <= 0) { return; }

        for (int i = 0; i < createdHotKey.Count; i++)
        {
            Destroy(createdHotKey[i]);
        }
    }

    void CloneAttackLogic()
    {
        if ((cloneAttackTimer < 0) && cloneAttackReleasted && (amountOfAtttack > 0))
        {
            cloneAttackTimer = cloneAttackCooldown;

            int randomIndex = Random.Range(0, targets.Count);

            float xOffset;

            if (Random.Range(0, 100) > 50)
                xOffset = 2;
            else
                xOffset = -2;

            if (targets[randomIndex] != null)
            {
                if (SkillManager.instance.clone.crystalInsteadUnlocked)
                {
                    SkillManager.instance.crystal.CreateCrystal();
                    SkillManager.instance.crystal.CurrentCrystalChooseRandomTarget();
                }
                else
                    SkillManager.instance.clone.CreateClone(targets[randomIndex], new Vector3(xOffset, 0f));

                amountOfAtttack--;
            }
            else
                FinishBlackHoleAbility();

            if (amountOfAtttack <= 0)
                Invoke(nameof(FinishBlackHoleAbility), 1f);
        }
    }

    void FinishBlackHoleAbility()
    {
        DestroyHotKeys();

        playerCanExitState = true;
        canShrink = true;
        cloneAttackReleasted = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTime(true);
            CreateHotKey(collision);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
            collision.GetComponent<Enemy>().FreezeTime(false);
    }

    void CreateHotKey(Collider2D collision)
    {
        if (keyCodeList.Count <= 0)
        {
            Debug.LogWarning("Not enough hot keys in a key code list (BlackHoleController)");
            return;
        }

        if (!canCreateHotKeys) { return; }

        GameObject newHotKey = Instantiate(hotKeyPref, collision.transform.position + new Vector3(0.25f, 1.8f), Quaternion.identity);
        createdHotKey.Add(newHotKey);

        KeyCode choosenKey = keyCodeList[Random.Range(0, keyCodeList.Count)];
        keyCodeList.Remove(choosenKey);

        BlackHoleHotKeyController newHotKeyScript = newHotKey.GetComponent<BlackHoleHotKeyController>();
        newHotKeyScript.SetupHotKey(choosenKey, collision.transform, this);
    }

    public void AddEnemyToList(Transform _enemyTransform) => targets.Add(_enemyTransform);
}
