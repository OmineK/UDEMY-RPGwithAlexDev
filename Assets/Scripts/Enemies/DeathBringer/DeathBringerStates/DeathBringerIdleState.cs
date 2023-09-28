using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerIdleState : EnemyState
{
    DeathBringer enemy;
    Transform player;

    public DeathBringerIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, DeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player.transform;
        stateTimer = enemy.idleTime;
    }

    public override void Update()
    {
        base.Update();

        if (Vector2.Distance(player.position, enemy.transform.position) > enemy.aggroDistance) { return; }

        if (stateTimer < 0)
        {
            if ((enemy.IsPlayerDetected().distance < enemy.attackDistance) && (enemy.IsPlayerDetected().distance != 0))
            {
                if (enemy.battleState.CanAttack())
                    stateMachine.ChangeState(enemy.attackState);
            }
            else
                stateMachine.ChangeState(enemy.battleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
