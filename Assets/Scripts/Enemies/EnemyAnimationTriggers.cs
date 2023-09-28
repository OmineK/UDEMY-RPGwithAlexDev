using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationTriggers : MonoBehaviour
{
    Enemy enemy => GetComponentInParent<Enemy>();

    void AnimationTrigger()
    {
        enemy.AnimationFinishTrigger();
    }

    void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {
                PlayerStats target = hit.GetComponent<PlayerStats>();
                enemy.stats.DoDamage(target);
            }
        }
    }

    void SpeicalAttackTrigger()
    {
        enemy.AnimationSpecialAttackTrigger();
    }

    void OpenCounterWindow() => enemy.OpenCounterAttackWindow();

    void CloseCounterWindow() => enemy.CloseCounterAttackWindow();
}
