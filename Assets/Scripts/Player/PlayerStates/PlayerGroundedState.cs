using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.R) && player.skill.blackHole.blackHoleUnlocked && (stateMachine.currentState != player.blackHoleState))
        {
            if (player.skill.blackHole.cooldownTimer > 0)
            {
                player.fx.CreatePopUpText("Cooldown");
                return;
            }

            stateMachine.ChangeState(player.blackHoleState);
        }

        if (Input.GetMouseButtonDown(1) && HasNoSword() && player.skill.swordThrow.swordThrowUnlocked && (stateMachine.currentState != player.blackHoleState))
            stateMachine.ChangeState(player.aimSwordState);


        if (Input.GetKeyDown(KeyCode.Q) && player.skill.parry.parryUnlocked && (stateMachine.currentState != player.blackHoleState))
        {
            if (SkillManager.instance.parry.CanUseSkill() == false)
            {
                player.fx.CreatePopUpText("Cooldown");
                return;
            }

            stateMachine.ChangeState(player.counterAttackState);
        }

        if (Input.GetKeyDown(KeyCode.Space) && player.isGroundDetected() && (stateMachine.currentState != player.blackHoleState))
            stateMachine.ChangeState(player.jumpState);

        if (Input.GetMouseButton(0) && HasNoSword() && (stateMachine.currentState != player.blackHoleState))
            stateMachine.ChangeState(player.primaryAttackState);

        if (!player.isGroundDetected())
            stateMachine.ChangeState(player.airState);
    }

    public override void Exit()
    {
        base.Exit();
    }

    bool HasNoSword()
    {
        if (!player.sword)
            return true;

        player.sword.GetComponent<SwordController>().ReturnSword();
        return false;
    }
}
