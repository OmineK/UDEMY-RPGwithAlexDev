using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    [Header("Soul drop")]
    [SerializeField] Stat soulsDropAmount;

    [Header("Level details")]
    [SerializeField] int level;

    float percentageModifier = 0.3f; //each level increse 30% stats UP

    Enemy enemy;
    ItemObjectDrop dropSystem;

    protected override void Start()
    {
        if (soulsDropAmount.GetValue() == 0)
            soulsDropAmount.SetDefaultValue(10);

        ApplyLevelModifiers();

        base.Start();

        enemy = GetComponent<Enemy>();
        dropSystem = GetComponent<ItemObjectDrop>();
    }

    void ApplyLevelModifiers()
    {
        Modify(soulsDropAmount);

        Modify(strength);
        Modify(agility);
        Modify(intelligence);
        Modify(vitality);

        Modify(damage);
        Modify(critChance);
        Modify(critPower);

        Modify(maxHealth);
        Modify(armor);
        Modify(evasion);
        Modify(magicResistance);

        Modify(fireDamage);
        Modify(iceDamage);
        Modify(lightningDamage);
    }

    void Modify(Stat _stat)
    {
        for (int i = 1; i < level; i++)
        {
            float modifier = _stat.GetValue() * percentageModifier;
            _stat.AddModifier(Mathf.RoundToInt(modifier));
        }
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
    }

    protected override void Die()
    {
        base.Die();
        enemy.Die();

        PlayerManager.instance.currency += soulsDropAmount.GetValue();
        dropSystem.GenerateDrop();

        Destroy(gameObject, 5f);
    }
}
