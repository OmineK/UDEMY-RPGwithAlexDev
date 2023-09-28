using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadyExplodeController : MonoBehaviour
{
    float growSpeed;
    float maxSize;
    float explodeRadius;

    bool canGrow = true;

    CharacterStats myStats;
    Animator anim;

    void Start()
    {
        AudioManager.instance.PlaySFX(29, null);
    }

    void Update()
    {
        if (canGrow)
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);

        if ((maxSize - transform.localScale.x) < 0.5f)
            anim.SetTrigger("Explode");
    }

    public void SetupExplode(CharacterStats _myStats, float _growSpeed, float _maxSize, float _radius)
    {
        anim = GetComponent<Animator>();

        myStats = _myStats;
        growSpeed = _growSpeed;
        maxSize = _maxSize;
        explodeRadius = _radius;
    }

    void AnimationExplodeEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explodeRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<CharacterStats>() != null)
            {
                hit.GetComponent<Entity>().SetupKnockbackDirection(transform);

                myStats.DoDamage(hit.GetComponent<CharacterStats>());
            }
        }
    }

    void SelfDestroy() => Destroy(this.gameObject);
}
