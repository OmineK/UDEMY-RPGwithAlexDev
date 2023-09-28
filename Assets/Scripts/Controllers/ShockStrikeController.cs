using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockStrikeController : MonoBehaviour
{
    [SerializeField] float speed;

    CharacterStats targetStats;
    Animator anim;

    int damage;
    bool triggered;

    public void Setup(int _damage, CharacterStats _targetStats)
    {
        targetStats = _targetStats;
        damage = _damage;
    }

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (triggered) { return; }
        if (!targetStats) { return; }

        transform.position = Vector2.MoveTowards(transform.position, targetStats.transform.position, speed * Time.deltaTime);
        transform.right = transform.position - targetStats.transform.position;

        if (Vector2.Distance(transform.position, targetStats.transform.position) < 0.1f)
        {
            anim.transform.localPosition = new Vector3(0, 0.4f);
            anim.transform.localRotation = Quaternion.identity;

            transform.localRotation = Quaternion.identity;
            transform.localScale = new Vector3(3, 3);

            triggered = true;
            anim.SetTrigger("Hit");
            Invoke(nameof(DamageAndSelfDestroy), 0.1f);
        }
    }

    void DamageAndSelfDestroy()
    {
        targetStats.ApplyShock(true);

        targetStats.TakeDamage(damage);
        Destroy(this.gameObject, 0.5f);
    }
}