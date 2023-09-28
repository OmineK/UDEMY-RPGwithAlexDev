using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellHandController : MonoBehaviour
{
    [SerializeField] Transform check;
    [SerializeField] Vector2 boxSize;
    [SerializeField] LayerMask whatIsPlayer;

    CharacterStats myStats;

    public void SetupSpell(CharacterStats _stats) => myStats = _stats;

    void AnimationTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(check.position, boxSize, whatIsPlayer);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<PlayerStats>() != null)
            {
                hit.GetComponent<Entity>().SetupKnockbackDirection(transform);
                myStats.DoDamage(hit.GetComponent<PlayerStats>());
            }
        }
    }

    void SelfDestroy() => Destroy(this.gameObject);

    void OnDrawGizmos() => Gizmos.DrawWireCube(check.position, boxSize);
}
