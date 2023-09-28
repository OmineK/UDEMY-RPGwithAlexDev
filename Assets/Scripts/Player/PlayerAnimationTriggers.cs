using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    Player player => GetComponentInParent<Player>();

    void AnimationTrigger()
    {
        player.AnimationTrigger();
    }

    void AttackTrigger()
    {
        AudioManager.instance.PlaySFX(2, null);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                EnemyStats target = hit.GetComponent<EnemyStats>();

                if (target != null)
                    player.stats.DoDamage(target);

                ItemDataEquipment weaponData = Inventory.instance.GetEquipment(EquipmentType.Weapon);

                if (weaponData != null)
                    weaponData.Effect(target.transform);
            }
        }
    }

    void ThrowSword()
    {
        SkillManager.instance.swordThrow.CreateSword();
    }
}
