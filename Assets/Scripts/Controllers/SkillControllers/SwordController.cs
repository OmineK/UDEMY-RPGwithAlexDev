using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    [Header("Spinning info")]
    float maxTravelDistance;
    float spinDir;
    float spinDuration;
    float spinTimer;
    bool wasStopped;
    bool isSpinning;

    [Header("Piercing info")]
    int pierceAmount;

    [Header("Bouncing info")]
    List<Transform> enemyTarget;
    float bounceSpeed;
    float hitTimer;
    float hitCooldown;
    bool isBouncing;
    int bounceAmount;
    int targetIndex;

    [Header("General info")]
    [Tooltip("Enemy freeze time duration after getting hit (from player skill - sword throw)")] float freezeTimeDuration;
    float returnSpeed;

    Animator anim;
    Rigidbody2D rb;
    CapsuleCollider2D cd;
    Player player;

    bool canRotate = true;
    bool isReturning;

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CapsuleCollider2D>();
    }

    void Update()
    {
        if (canRotate)
            transform.right = rb.velocity;

        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, player.transform.position) < 1f)
                player.CatchTheSword();
        }

        BounceLogic();
        SpinLogic();
    }

    void BounceLogic()
    {
        if (isBouncing && enemyTarget.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTarget[targetIndex].position, bounceSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < 0.1f)
            {
                SwordThrowSkillDamage(enemyTarget[targetIndex].GetComponent<Enemy>());

                targetIndex++;
                bounceAmount--;

                if (bounceAmount <= 0)
                {
                    isBouncing = false;
                    isReturning = true;
                }

                if (targetIndex >= enemyTarget.Count)
                    targetIndex = 0;
            }
        }
    }

    void SpinLogic()
    {
        if (isSpinning)
        {
            if (Vector2.Distance(player.transform.position, transform.position) > maxTravelDistance)
            {
                StopWhenSpinning();
            }

            if (wasStopped)
            {
                spinTimer -= Time.deltaTime;

                transform.position = Vector2.MoveTowards(transform.position,
                    new Vector2(transform.position.x + spinDir, transform.position.y),
                    1.5f * Time.deltaTime);

                if (spinTimer < 0)
                {
                    isReturning = true;
                    isSpinning = false;
                }

                hitTimer -= Time.deltaTime;

                if (hitTimer < 0)
                {
                    hitTimer = hitCooldown;

                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1f);

                    foreach (var hit in colliders)
                    {
                        if (hit.GetComponent<Enemy>() != null)
                            SwordThrowSkillDamage(hit.GetComponent<Enemy>());
                    }
                }
            }
        }
    }

    void StopWhenSpinning()
    {
        if (wasStopped == true) { return; }

        wasStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        spinTimer = spinDuration;
    }

    public void SetupSword(Vector2 _dir, float _gravityScale, Player _player, float _freezeTimeDuration, float _returnSpeed)
    {
        player = _player;
        freezeTimeDuration = _freezeTimeDuration;
        returnSpeed = _returnSpeed;

        rb.velocity = _dir;
        rb.gravityScale = _gravityScale;

        if (pierceAmount <= 0)
            anim.SetBool("Rotation", true);

        spinDir = Mathf.Clamp(rb.velocity.x, -1, 1);

        Invoke("DestroyMe", 7f);
    }

    void DestroyMe()
    {
        Destroy(this.gameObject);
    }

    public void SetupBounce(bool _isBouncing, int _bounceAmount, float _bounceSpeed)
    {
        isBouncing = _isBouncing;
        bounceAmount = _bounceAmount;
        bounceSpeed = _bounceSpeed;

        enemyTarget = new List<Transform>();
    }

    public void SetupPierce(int _pierceAmount)
    {
        pierceAmount = _pierceAmount;
    }

    public void SetupSpin(bool _isSpinning, float _maxTravelDistance, float _spinDuration, float _hitCooldown)
    {
        isSpinning = _isSpinning;
        maxTravelDistance = _maxTravelDistance;
        spinDuration = _spinDuration;
        hitCooldown = _hitCooldown;
    }

    public void ReturnSword()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = null;
        isReturning = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturning) { return; }

        if (collision.GetComponent<Enemy>() != null)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            SwordThrowSkillDamage(enemy);
        }

        SetupTargetsForBounce(collision);

        StuckInto(collision);
    }

    void SwordThrowSkillDamage(Enemy enemy)
    {
        EnemyStats enemyStats = enemy.GetComponent<EnemyStats>();

        player.stats.DoDamage(enemyStats);

        if (player.skill.swordThrow.timeStopUnlocked)
            enemy.FreezeTimeFor(freezeTimeDuration);

        if (player.skill.swordThrow.vulnerableUnlocked)
            enemyStats.MakeVulnerableFor(freezeTimeDuration);

        ItemDataEquipment equipedAmulet = Inventory.instance.GetEquipment(EquipmentType.Amulet);

        if (equipedAmulet != null)
            equipedAmulet.Effect(enemy.transform);
    }

    void SetupTargetsForBounce(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            if (isBouncing && enemyTarget.Count <= 0)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);

                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<Enemy>() != null)
                        enemyTarget.Add(hit.transform);
                }
            }
        }
    }

    void StuckInto(Collider2D collision)
    {
        if (pierceAmount > 0 && (collision.GetComponent<Enemy>() != null))
        {
            pierceAmount--;
            return;
        }

        if (isSpinning && (collision.GetComponent<Enemy>() != null))
        {
            StopWhenSpinning();
            return;
        }

        if (isSpinning && (collision.GetComponent<Enemy>() == null))
        {
            ReturnSword();
            return;
        }

        canRotate = false;
        cd.enabled = false;

        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        GetComponentInChildren<ParticleSystem>().Play();

        if (isBouncing && enemyTarget.Count > 0) { return; }

        anim.SetBool("Rotation", false);
        transform.parent = collision.transform;
    }
}