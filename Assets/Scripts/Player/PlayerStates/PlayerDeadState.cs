using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadState : PlayerState
{
    public PlayerDeadState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        GameObject.Find("Canvas").GetComponent<UI>().SwitchToEndScreen();

        AudioManager.instance.PlaySFX(11, null);
        AudioManager.instance.PlaySFX(34, null);

        AudioManager.instance.playBGM = false;
    }

    public override void Update()
    {
        base.Update();
        player.SetZeroVelocity();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
