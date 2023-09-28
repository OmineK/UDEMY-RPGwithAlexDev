using UnityEngine;

public class PlayerBlackHoleState : PlayerState
{
    float flyTime = 0.4f;
    float defaultGravityScale;
    bool skillUsed;

    public PlayerBlackHoleState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        defaultGravityScale = rb.gravityScale;

        skillUsed = false;
        stateTimer = flyTime;
        rb.gravityScale = 0f;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer > 0)
        {
            rb.velocity = new Vector2(0, 15);
        }

        if (stateTimer < 0)
        {
            rb.velocity = new Vector2(0, -0.1f);

            if (!skillUsed)
            {
                if (player.skill.blackHole.CanUseSkill())
                    skillUsed = true;
            }
        }

        if (player.skill.blackHole.SkillCompleted())
            stateMachine.ChangeState(player.airState);
    }

    public override void Exit()
    {
        base.Exit();

        //////////////////////////////////////////////////////////////////////////////////
        ////Exit from that state is in blackhole controller, when all attacks are over////
        //////////////////////////////////////////////////////////////////////////////////

        rb.gravityScale = defaultGravityScale;
        player.fx.MakeTransparent(false);
    }
}
