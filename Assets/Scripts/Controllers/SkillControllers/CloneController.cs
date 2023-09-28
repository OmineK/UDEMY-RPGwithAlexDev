using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneController : MonoBehaviour
{
    [SerializeField] float colorLoosingSpeed;
    [SerializeField] float attackCheckRadius;
    [SerializeField] Transform attackCheck;

    float cloneTimer;
    float chanceToDuplicateClone;
    float cloneAttackMultiplier;
    bool canDuplicateClone;
    int facingDir = 1;

    SpriteRenderer sr;
    Animator anim;
    Transform closestEnemy;
    Player player;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        cloneTimer -= Time.deltaTime;

        if (cloneTimer < 0)
            sr.color = new Color(1, 1, 1, sr.color.a - (Time.deltaTime * colorLoosingSpeed));

        if (sr.color.a <= 0)
            Destroy(this.gameObject);
    }

    public void SetupClone(Transform _newTransform, float _cloneDuration, bool _canAttack, 
        Vector3 _offset, Transform _closestEnemy, bool _canDuplicate, float _chanceToDuplicateClone, 
        Player _player, float _cloneAttackMultiplier)
    {
        if (_canAttack)
            anim.SetInteger("AttackNumber", Random.Range(1, 4));

        cloneAttackMultiplier = _cloneAttackMultiplier;
        player = _player;

        transform.position = _newTransform.position + _offset;
        cloneTimer = _cloneDuration;

        closestEnemy = _closestEnemy;
        canDuplicateClone = _canDuplicate;
        chanceToDuplicateClone = _chanceToDuplicateClone;
        FaceClosestTarget();
    }

    void AnimationTrigger()
    {
        cloneTimer = -0.1f;
    }

    void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                // player.stats.DoDamage(hit.GetComponent<CharacterStats>());

                hit.GetComponent<Entity>().SetupKnockbackDirection(transform);

                PlayerStats playerStats = player.GetComponent<PlayerStats>();
                EnemyStats enemyStats = hit.GetComponent<EnemyStats>();

                playerStats.CloneDoDamage(enemyStats, cloneAttackMultiplier);

                if (player.skill.clone.canApplyOnHitEffect)
                {
                    ItemDataEquipment weaponData = Inventory.instance.GetEquipment(EquipmentType.Weapon);

                    if (weaponData != null)
                        weaponData.Effect(hit.transform);
                }

                if (canDuplicateClone)
                {
                    if (Random.Range(0,101) < chanceToDuplicateClone)
                    {
                        SkillManager.instance.clone.CreateClone(hit.transform, new Vector3(0.5f * facingDir, 0));
                    }
                }
            }

        }
    }

    void FaceClosestTarget()
    {
        if (closestEnemy != null)
        {
            if (transform.position.x > closestEnemy.position.x)
            {
                facingDir = -1;
                transform.Rotate(0, 180, 0);
            }
        }
    }
}
