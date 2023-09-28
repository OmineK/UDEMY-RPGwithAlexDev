using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadyBattleState : EnemyState
{
    Shady enemy;
    Transform player;

    int moveDir;

    public ShadyBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Shady _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player.transform;

        if (player.GetComponent<PlayerStats>().isDead)
            stateMachine.ChangeState(enemy.moveState);
    }

    public override void Update()
    {
        base.Update();

        if (enemy.IsPlayerDetected())
        {
            stateTimer = enemy.battleTime;

            if (enemy.IsPlayerDetected().distance < enemy.attackDistance)
                enemy.stats.KillEntity(); //enter to dead state + trigger explode + drop items/souls
        }
        else
        {
            if (stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > 10)
                stateMachine.ChangeState(enemy.idleState);
        }

        if (enemy.isGroundDetected() == false)
        {
            enemy.Flip();
            stateMachine.ChangeState(enemy.idleState);
            return;
        }

        if (player.position.x > enemy.transform.position.x)
            moveDir = 1;
        else if (player.position.x < enemy.transform.position.x)
            moveDir = -1;

        if (Vector2.Distance(player.transform.position, enemy.transform.position) < (enemy.attackDistance * 0.9f))
            enemy.SetVelocity(enemy.moveSpeed * 0.75f * moveDir, rb.velocity.y);
        else
            enemy.SetVelocity(enemy.moveSpeed * 1.5f * moveDir, rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }

    bool CanAttack()
    {
        if (Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown)
        {
            enemy.attackCooldown = Random.Range(enemy.minAttackCooldown, enemy.maxAttackCooldown);
            enemy.lastTimeAttacked = Time.time;
            return true;
        }

        return false;
    }
}
