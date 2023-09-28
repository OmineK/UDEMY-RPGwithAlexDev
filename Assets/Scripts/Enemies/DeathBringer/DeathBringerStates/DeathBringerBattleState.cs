using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerBattleState : EnemyState
{
    DeathBringer enemy;
    Transform player;

    int moveDir;

    public DeathBringerBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, DeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player.transform;

        if (player.GetComponent<PlayerStats>().isDead)
            stateMachine.ChangeState(enemy.idleState);
    }

    public override void Update()
    {
        base.Update();

        if (enemy.IsPlayerDetected())
        {
            stateTimer = enemy.battleTime;

            if (enemy.IsPlayerDetected().distance < enemy.attackDistance)
            {
                if (CanAttack())
                    stateMachine.ChangeState(enemy.attackState);
                else
                    stateMachine.ChangeState(enemy.idleState);
            }
        }

        // if enemy stuck UNDER player
        if ((Mathf.RoundToInt(player.transform.position.x) == Mathf.RoundToInt(enemy.transform.position.x)) &&
            (player.transform.position.y - enemy.transform.position.y) > 5 &&
            player.transform.position.y > enemy.transform.position.y)
        {
            enemy.canTeleportToPlayer = true;
            stateMachine.ChangeState(enemy.teleportState);
        }

        // if enemy stuck ABOVE player
        if ((Mathf.RoundToInt(player.transform.position.x) == Mathf.RoundToInt(enemy.transform.position.x)) &&
            (enemy.transform.position.y - player.transform.position.y) > 8 &&
            player.transform.position.y < enemy.transform.position.y)
        {
            enemy.canTeleportToPlayer = true;
            stateMachine.ChangeState(enemy.teleportState);
        }

        if (player.position.x > enemy.transform.position.x)
            moveDir = 1;
        else if (player.position.x < enemy.transform.position.x)
            moveDir = -1;

        if (enemy.IsPlayerDetected() && (enemy.IsPlayerDetected().distance < enemy.attackDistance))
            return;

        enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public bool CanAttack()
    {
        if (Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown)
        {
            enemy.attackCooldown = Random.Range(enemy.minAttackCooldown, enemy.maxAttackCooldown);
            return true;
        }

        return false;
    }
}
