using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if (player.isWallDetected())
            stateMachine.ChangeState(player.wallSlideState);

        if (player.isGroundDetected())
            stateMachine.ChangeState(player.idleState);

        if (xInput != 0 && !player.isWallDetected())
            player.SetVelocity(player.moveSpeed * 0.8f * xInput, rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
