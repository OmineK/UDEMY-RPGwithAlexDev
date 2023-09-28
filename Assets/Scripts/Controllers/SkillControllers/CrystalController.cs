using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalController : MonoBehaviour
{
    [SerializeField] LayerMask whatIsEnemy;

    Animator anim => GetComponent<Animator>();
    CircleCollider2D cd => GetComponent<CircleCollider2D>();
    Transform closestTarget;
    Player player;

    float crystalExistTimer;
    float moveSpeed;
    float growSpeed = 5f;

    bool canExplode;
    bool canMove;
    bool canGrow;

    void Start()
    {
        AudioManager.instance.PlaySFX(26, null);
    }

    public void SetupCrystal(float _crystalDuration, bool _canExplode, bool _canMove, float _moveSpeed, Transform _closestTarget, Player _player)
    {
        crystalExistTimer = _crystalDuration;
        canExplode = _canExplode;
        canMove = _canMove;
        moveSpeed = _moveSpeed;
        closestTarget = _closestTarget;
        player = _player;
    }

    void Update()
    {
        crystalExistTimer -= Time.deltaTime;

        if (crystalExistTimer < 0)
        {
            FinishCrystal();
        }

        if (canMove)
        {
            if (closestTarget == null) { return; }

            transform.position = Vector2.MoveTowards(transform.position, closestTarget.position, moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, closestTarget.position) < 1f)
            {
                FinishCrystal();
                canMove = false;
            }
        }

        if (canGrow)
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(3f, 3f), growSpeed * Time.deltaTime);
    }

    public void FinishCrystal()
    {
        if (canExplode)
        {
            canGrow = true;
            anim.SetTrigger("Explode");
            AudioManager.instance.PlaySFX(25, transform);
        }
        else
            SelfDestroy();
    }

    public void ChooseRandomEnemy()
    {
        float radius = SkillManager.instance.blackHole.GetBlackHoleRadius();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, whatIsEnemy);

        if (colliders.Length > 0)
            closestTarget = colliders[Random.Range(0, colliders.Length)].transform;
    }

    void AnimationExplodeEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, cd.radius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Entity>().SetupKnockbackDirection(transform);

                player.stats.DoMagicalDamage(hit.GetComponent<CharacterStats>());

                ItemDataEquipment equipedAmulet = Inventory.instance.GetEquipment(EquipmentType.Amulet);

                if (equipedAmulet != null)
                    equipedAmulet.Effect(hit.transform);
            }
        }
    }

    void SelfDestroy() => Destroy(this.gameObject);
}
