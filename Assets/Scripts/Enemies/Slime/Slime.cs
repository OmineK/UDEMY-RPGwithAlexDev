using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SlimeType { big, small}

public class Slime : Enemy
{
    [Header("Slime specific")]
    [SerializeField] SlimeType slimeType;
    [SerializeField] GameObject slimePref;
    [SerializeField] Vector2 minCreationVelocity;
    [SerializeField] Vector2 maxCreationVelocity;
    int slimesToCreate = 3;

    #region States
    public SlimeIdleState idleState { get; private set; }
    public SlimeMoveState moveState { get; private set; }
    public SlimeBattleState battleState { get; private set; }
    public SlimeAttackState attackState { get; private set; }
    public SlimeStunnedState stunnedState { get; private set; }
    public SlimeDeadState deadState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        idleState = new SlimeIdleState(this, stateMachine, "Idle", this);
        moveState = new SlimeMoveState(this, stateMachine, "Move", this);
        battleState = new SlimeBattleState(this, stateMachine, "Move", this);
        attackState = new SlimeAttackState(this, stateMachine, "Attack", this);
        stunnedState = new SlimeStunnedState(this, stateMachine, "Stunned", this);
        deadState = new SlimeDeadState(this, stateMachine, "Idle", this);
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
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

        if (slimeType == SlimeType.small) { return; }

        CreateSlimes();
    }

    void CreateSlimes()
    {
        for (int i = 0; i < slimesToCreate; i++)
        {
            GameObject newSlime = Instantiate(slimePref, transform.position, Quaternion.identity);

            newSlime.GetComponent<Slime>().SetupSlime(facingDir);
        }
    }

    public void SetupSlime(int _facingDir)
    {
        if (_facingDir != facingDir)
            Flip();

        float xVelocity = Random.Range(minCreationVelocity.x, maxCreationVelocity.x);
        float yVelocity = Random.Range(minCreationVelocity.y, maxCreationVelocity.y);

        isKnocked = true;

        GetComponent<Rigidbody2D>().velocity = new Vector2(xVelocity * -facingDir, yVelocity);

        Invoke(nameof(CancelKnockback), 1.5f);
    }

    void CancelKnockback() => isKnocked = false;
}
