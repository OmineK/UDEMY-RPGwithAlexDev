using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerHandSpellCastState : EnemyState
{
    DeathBringer enemy;

    float spellTimer;
    int amountOfSpells;

    public DeathBringerHandSpellCastState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, DeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        spellTimer = 0.5f;
        amountOfSpells = enemy.amountOfSpells;
    }

    public override void Update()
    {
        base.Update();
        spellTimer -= Time.deltaTime;

        if (CanCast())
            enemy.CastHandSpell();
        

        if (amountOfSpells <= 0)
            stateMachine.ChangeState(enemy.teleportState);
    }

    public override void Exit()
    {
        base.Exit();

        enemy.lastTimeCast = Time.time;
    }

    bool CanCast()
    {
        if ((amountOfSpells > 0) && (spellTimer < 0))
        {
            amountOfSpells--;
            spellTimer = enemy.castingSpeed;
            return true;
        }

        return false;
    }
}
