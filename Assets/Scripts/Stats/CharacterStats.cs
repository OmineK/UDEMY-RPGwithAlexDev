using System.Collections;
using UnityEngine;

public enum StatType
{
    strength,
    agility,
    intelligence,
    vitality,
    damage,
    critChance,
    critPower,
    health,
    armor,
    evasion,
    magicResistance,
    fireDamage,
    iceDamage,
    lightningDamage,
}

public class CharacterStats : MonoBehaviour
{
    [Header("Major stats")]
    public Stat strength; //+1 dmg, +1% crit power
    public Stat agility; //+1 evasion, +1% crit chance
    public Stat intelligence; //+1 magic dmg, +3 magic resistance
    public Stat vitality; //+5 maximum health

    [Header("Offensive stats")]
    public Stat damage;
    [Tooltip("Enter value in (1-100) %")] public Stat critChance;
    [Tooltip("Enter value 100+, that number will calculate with total dmg and add result to it")] public Stat critPower;

    [Header("Defensive stats")]
    public Stat maxHealth;
    public Stat armor; //physical resistance
    public Stat evasion;
    public Stat magicResistance;
    [Space]
    public int currentHealth;

    [Header("Magic stats")]
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightningDamage;
    [Space]
    public bool isIgnited; //does dmg over time
    public bool isChilled; //reduce armor by 20% and slow down by 30%
    public bool isShocked; //reduce evasion by 20%

    [Header("Ignite effect info")]
    [SerializeField] float igniteDuration;
    int igniteDamage;
    float ignitedTimer;
    float igniteDamageCooldown = 0.3f;
    float igniteDamageTimer;

    [Header("Chill effect info")]
    [SerializeField] float chillDuration;
    float chilledTimer;

    [Header("Shock effect info")]
    [SerializeField] GameObject ShockStrikePref;
    [SerializeField] float shockDuration;
    int shockDamage;
    float shockedTimer;

    public System.Action onHealthChanged;
    public bool isDead { get; private set; }
    public bool isInvincible { get; private set; }

    bool isVulnerable;

    EntityFX fx;

    protected virtual void Start()
    {
        critPower.SetDefaultValue(150);
        currentHealth = GetMaxHealthValue();

        fx = GetComponent<EntityFX>();
    }

    protected virtual void Update()
    {
        ignitedTimer -= Time.deltaTime;
        chilledTimer -= Time.deltaTime;
        shockedTimer -= Time.deltaTime;

        igniteDamageTimer -= Time.deltaTime;

        if (ignitedTimer < 0)
            isIgnited = false;

        if (chilledTimer < 0)
            isChilled = false;

        if (shockedTimer < 0)
            isShocked = false;

        if (isIgnited)
            ApplyIgniteDamage();
    }

    public virtual void IncreaseStatBy(int _modifier, float _duration, Stat _statToModify)
    {
        StartCoroutine(StatModCoroutine(_modifier, _duration, _statToModify));
    }

    IEnumerator StatModCoroutine(int _modifier, float _duration, Stat _statToModify)
    {
        _statToModify.AddModifier(_modifier);
        yield return new WaitForSeconds(_duration);
        _statToModify.RemoveModifier(_modifier);
    }

    public void MakeVulnerableFor(float _duration) => StartCoroutine(VulnerableCoroutine(_duration));

    IEnumerator VulnerableCoroutine(float _vulnerableDuration)
    {
        isVulnerable = true;
        yield return new WaitForSeconds(_vulnerableDuration);
        isVulnerable = false;
    }

    public virtual void DoDamage(CharacterStats _targetStats)
    {
        if (TargetCanAvoidAttack(_targetStats)) { return; }
        if (_targetStats.isInvincible) { return; }

        _targetStats.GetComponent<Entity>().SetupKnockbackDirection(transform);

        int totalDamage = damage.GetValue() + strength.GetValue();

        if (CanCrit())
            totalDamage = CalculateCriticalDamage(totalDamage);

        fx.CreateHitFX(_targetStats.transform, CanCrit());

        totalDamage = CheckTargetArmor(_targetStats, totalDamage);

        _targetStats.TakeDamage(totalDamage);
        DoMagicalDamage(_targetStats);
    }

    public virtual void TakeDamage(int _damage)
    {
        if (isInvincible) { return; }

        DecreaseHealthBy(_damage);

        GetComponent<Entity>().DamageImpact();
        fx.StartCoroutine("FlashFX");

        if (currentHealth <= 0 && !isDead)
            Die();
    }

    public virtual void IncreaseHealthBy(int _healAmount)
    {
        currentHealth += _healAmount;

        if (currentHealth > GetMaxHealthValue())
            currentHealth = GetMaxHealthValue();

        if (onHealthChanged != null)
            onHealthChanged();
    }

    protected virtual void DecreaseHealthBy(int _damage)
    {
        if (isVulnerable)
            _damage = Mathf.RoundToInt(_damage * 1.2f);

        currentHealth -= _damage;

        if (onHealthChanged != null)
            onHealthChanged();
    }

    protected virtual void Die()
    {
        isDead = true;
    }

    public void KillEntity()
    {
        if (!isDead)
            Die();
    }

    public void MakeInvincible(bool _invincible) => isInvincible = _invincible;


    #region Stats calculations

    protected bool TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        if (isShocked)
            totalEvasion = Mathf.RoundToInt(totalEvasion * 0.8f); //decrease 20% of evasion

        if (Random.Range(1, 101) < totalEvasion)
        {
            _targetStats.OnEvasion();
            return true;
        }

        return false;
    }

    public virtual void OnEvasion()
    {
        //TODO in override
    }

    protected bool CanCrit()
    {
        int totalCriticalChance = critChance.GetValue() + agility.GetValue();

        if (Random.Range(0, 101) <= totalCriticalChance)
            return true;

        return false;
    }

    protected int CalculateCriticalDamage(int _damage)
    {
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * 0.01f;
        float totalCritDamage = _damage * totalCritPower;

        return Mathf.RoundToInt(totalCritDamage);
    }

    protected int CheckTargetArmor(CharacterStats _targetStats, int totalDamage)
    {
        if (_targetStats.isChilled)
            totalDamage -= Mathf.RoundToInt(_targetStats.armor.GetValue() * 0.8f);
        else
            totalDamage -= _targetStats.armor.GetValue();

        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }

    int CheckTargetMagicalResistance(CharacterStats _targetStats, int totalMagicalDamage)
    {
        totalMagicalDamage -= _targetStats.magicResistance.GetValue() + (_targetStats.intelligence.GetValue() * 3);
        totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue);
        return totalMagicalDamage;
    }

    public int GetMaxHealthValue()
    {
        return maxHealth.GetValue() + (vitality.GetValue() * 5);
    }

    #endregion

    #region Magical dmg and ailments

    public virtual void DoMagicalDamage(CharacterStats _targetStats)
    {
        int fireDmg = fireDamage.GetValue();
        int iceDmg = iceDamage.GetValue();
        int lightningDmg = lightningDamage.GetValue();

        int totalMagicalDamage = fireDmg + iceDmg + lightningDmg + intelligence.GetValue();
        totalMagicalDamage = CheckTargetMagicalResistance(_targetStats, totalMagicalDamage);

        _targetStats.TakeDamage(totalMagicalDamage);

        if (Mathf.Max(fireDmg, iceDmg, lightningDmg) <= 0) { return; }

        AttemptToApplyAilments(_targetStats, fireDmg, iceDmg, lightningDmg);
    }

    void AttemptToApplyAilments(CharacterStats _targetStats, int fireDmg, int iceDmg, int lightningDmg)
    {
        bool canApplyIgnite = fireDmg > iceDmg && fireDmg > lightningDmg;
        bool canApplyChill = iceDmg > fireDmg && iceDmg > lightningDmg;
        bool canApplyShock = lightningDmg > fireDmg && lightningDmg > iceDmg;

        while (!canApplyIgnite && !canApplyChill && !canApplyShock)
        {
            if (Random.value < 0.3f && fireDmg > 0)
            {
                canApplyIgnite = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }

            if (Random.value < 0.4f && iceDmg > 0)
            {
                canApplyChill = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }

            if (Random.value < 0.5f && lightningDmg > 0)
            {
                canApplyShock = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
        }

        if (canApplyIgnite)
            _targetStats.SetupIgniteDamage(Mathf.RoundToInt(fireDmg * 0.15f));

        if (canApplyShock)
            _targetStats.SetupShockStrikeDamage(Mathf.RoundToInt(lightningDmg * 0.1f));

        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
    }

    public void ApplyAilments(bool _ignite, bool _chill, bool _shock)
    {
        bool canApplyIgnite = !isIgnited && !isChilled && !isShocked;
        bool canApplyChill = !isIgnited && !isChilled && !isShocked;
        bool canApplyShock = !isIgnited && !isChilled;

        if (_ignite && canApplyIgnite)
        {
            isIgnited = _ignite;
            ignitedTimer = igniteDuration;

            fx.IgniteFxFor(igniteDuration);
        }

        if (_chill && canApplyChill)
        {
            isChilled = _chill;
            chilledTimer = chillDuration;

            float slowPercentage = 0.3f;
            GetComponent<Entity>().SlowEntityBy(slowPercentage, chillDuration);

            fx.ChillFxFor(chillDuration);
        }

        if (_shock && canApplyShock)
        {
            if (!isShocked)
            {
                ApplyShock(_shock);
            }
            else
            {
                if (GetComponent<Player>() != null) { return; }

                HitClosestTargetWithShockStrike();
            }
        }
    }

    public void ApplyShock(bool _shock)
    {
        if (isShocked) { return; }

        isShocked = _shock;
        shockedTimer = shockDuration;

        fx.ShockFxFor(shockDuration);
    }

    void HitClosestTargetWithShockStrike()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);

        float closestDis = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (var hit in colliders)
        {
            if ((hit.GetComponent<Enemy>() != null) && (Vector2.Distance(transform.position, hit.transform.position) > 1))
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);

                if (distanceToEnemy < closestDis)
                {
                    closestDis = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }

            if (closestEnemy == null)
                closestEnemy = transform;
        }

        if (closestEnemy != null)
        {
            GameObject newShockStrike = Instantiate(ShockStrikePref, transform.position, Quaternion.identity);

            newShockStrike.GetComponent<ShockStrikeController>().Setup(shockDamage, closestEnemy.GetComponent<CharacterStats>());
        }
    }

    void ApplyIgniteDamage()
    {
        if (igniteDamageTimer < 0)
        {
            DecreaseHealthBy(igniteDamage);

            if (currentHealth < 0 && !isDead)
                Die();

            igniteDamageTimer = igniteDamageCooldown;
        }
    }

    public void SetupIgniteDamage(int _damage)
    {
        igniteDamage = _damage;
    }

    public void SetupShockStrikeDamage(int _damage) => shockDamage = _damage;

    #endregion

}