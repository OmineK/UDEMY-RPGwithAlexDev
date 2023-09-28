using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerTeleportState : EnemyState
{
    DeathBringer enemy;

    public DeathBringerTeleportState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, DeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.stats.MakeInvincible(true);
    }

    public override void Update()
    {
        base.Update();

        //if (enemy.canTeleportToPlayer) 
        //{
        //    stateMachine.ChangeState(enemy.battleState);
        //    return; 
        //}

        float chanceToCastSpellHand = Random.Range(1, 101);

        if (triggerCalled)
        {
            if (enemy.CanCastSpellHand() && (chanceToCastSpellHand <= 60))
                stateMachine.ChangeState(enemy.castState);
            else
                stateMachine.ChangeState(enemy.battleState);
        }
    }

    public override void Exit()
    {
        base.Exit();

        enemy.canTeleportToPlayer = false;
        enemy.stats.MakeInvincible(false);
    }
}
