using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.skill.dash.CreateCloneOnDash();

        stateTimer = player.dashDuration;

        player.stats.MakeInvincible(true);
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(player.dashSpeed * player.dashDir, 0);

        if (stateTimer < 0)
            stateMachine.ChangeState(player.idleState);

        if (!player.isGroundDetected() && player.isWallDetected())
            stateMachine.ChangeState(player.wallSlideState);

        player.fx.CreateAfterImage();
    }

    public override void Exit()
    {
        base.Exit();

        player.skill.dash.CreateCloneOnArrival();

        player.SetVelocity(0, rb.velocity.y);

        player.stats.MakeInvincible(false);
    }
}
