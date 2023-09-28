using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimSwordState : PlayerState
{
    public PlayerAimSwordState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.skill.swordThrow.DotsActive(true);
    }

    public override void Update()
    {
        base.Update();

        player.SetZeroVelocity();

        if (Input.GetMouseButtonUp(1))
            stateMachine.ChangeState(player.idleState);

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (player.transform.position.x > mousePos.x && player.facingDir == 1)
            player.Flip();
        else if (player.transform.position.x < mousePos.x && player.facingDir == -1)
            player.Flip();
    }

    public override void Exit()
    {
        base.Exit();

        AudioManager.instance.PlaySFX(27, null);
        player.StartCoroutine("BusyFor", 0.2f);
    }

}
