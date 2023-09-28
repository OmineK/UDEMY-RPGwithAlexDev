using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerDeadState : EnemyState
{
    DeathBringer enemy;

    public DeathBringerDeadState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, DeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        AudioManager.instance.PlaySFX(19, null);

        enemy.anim.SetBool("Stunned", true);

        enemy.anim.speed = 0;
        enemy.cd.enabled = false;

        stateTimer = 0.52f;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer > 0)
            rb.velocity = new Vector2(0, 12);

    }

    public override void Exit()
    {
        base.Exit();
    }
}
