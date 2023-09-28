using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringer : Enemy
{
    [Header("Teleport details")]
    [SerializeField] BoxCollider2D arena;
    [SerializeField] Vector2 teleportCheck;
    [Space]
    [Tooltip("Chance to teleport in percents")][Range(1, 100)][SerializeField] float defaultChanceToTeleport = 1;
    public float currentChanceToTeleport;
    public bool canTeleportToPlayer;

    [Header("Spell hand cast details")]
    [SerializeField] GameObject spellhandPref;
    [SerializeField] float spellCooldown;
    public float castingSpeed;
    public float lastTimeCast;
    public int amountOfSpells;

    #region States
    public DeathBringerIdleState idleState { get; private set; }
    public DeathBringerBattleState battleState { get; private set; }
    public DeathBringerAttackState attackState { get; private set; }
    public DeathBringerDeadState deadState { get; private set; }
    public DeathBringerHandSpellCastState castState { get; private set; }
    public DeathBringerTeleportState teleportState { get; private set; }
    public DeathBringerStunnedState stunnedState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        idleState = new DeathBringerIdleState(this, stateMachine, "Idle", this);
        battleState = new DeathBringerBattleState(this, stateMachine, "Move", this);
        attackState = new DeathBringerAttackState(this, stateMachine, "Attack", this);
        deadState = new DeathBringerDeadState(this, stateMachine, "Idle", this);
        castState = new DeathBringerHandSpellCastState(this, stateMachine, "Cast", this);
        teleportState = new DeathBringerTeleportState(this, stateMachine, "Teleport", this);

        stunnedState = new DeathBringerStunnedState(this, stateMachine, "Stunned", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
        currentChanceToTeleport = defaultChanceToTeleport;
    }

    public void CastHandSpell()
    {
        Player player = PlayerManager.instance.player;

        float xOffset = 0;

        if (player.rb.velocity.x != 0)
            xOffset = player.facingDir * 2.5f;
        else
            xOffset = Random.Range(-0.4f, 0.4f);

        Vector3 spellHandPosition = new Vector3(player.transform.position.x + xOffset, player.transform.position.y + 2.3f);

        GameObject newSpellHand = Instantiate(spellhandPref, spellHandPosition, Quaternion.identity);

        newSpellHand.GetComponent<SpellHandController>().SetupSpell(stats);
    }

    public override bool CanBeStunned()
    {
        if (base.CanBeStunned())
        {
            stateMachine.ChangeState(stunnedState);
            return true;
        }

        return false;
    }

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
    }

    public void FindPosition()
    {
        if (canTeleportToPlayer)
        {
            float playerX = PlayerManager.instance.player.transform.position.x;
            float playerY = PlayerManager.instance.player.transform.position.y;

            float xOffset = Random.Range(-2f, 2f);
            float yOffset = 1.5f;

            transform.position = new Vector3(playerX + xOffset, ((playerY + yOffset) - GroundBelow().distance + (cd.size.y / 2)));

            if (!GroundBelow() || SomethingIsAround() || transform.position.y > -29.5f)
                FindPosition();
        }
        else
        {
            float x = Random.Range(arena.bounds.min.x + 3, arena.bounds.max.x - 3);
            float y = Random.Range(arena.bounds.min.y + 3, arena.bounds.max.y - 3);

            transform.position = new Vector3(x, y);
            transform.position = new Vector3(transform.position.x, (transform.position.y - GroundBelow().distance + (cd.size.y / 2)));

            if (!GroundBelow() || SomethingIsAround())
                FindPosition();
        }
    }

    RaycastHit2D GroundBelow() => Physics2D.Raycast(transform.position, Vector2.down, 100, whatIsGround);

    bool SomethingIsAround() => Physics2D.BoxCast(transform.position, teleportCheck, 0, Vector2.zero, 0, whatIsGround);

    public bool CanTeleport()
    {
        if (Random.Range(1, 101) <= currentChanceToTeleport)
        {
            currentChanceToTeleport = defaultChanceToTeleport;
            return true;
        }

        return false;
    }

    public bool CanCastSpellHand()
    {

        if (Time.time >= lastTimeCast + spellCooldown)
            return true;

        return false;

    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y, -GroundBelow().distance));
        Gizmos.DrawWireCube(transform.position, teleportCheck);
    }
}
