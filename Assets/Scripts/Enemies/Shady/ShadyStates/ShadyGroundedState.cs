using UnityEngine;

public class ShadyGroundedState : EnemyState
{
    protected Shady enemy;
    protected Transform player;

    public ShadyGroundedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Shady _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player.transform;
    }
    public override void Update()
    {
        base.Update();

        if (enemy.IsPlayerDetected() || Vector2.Distance(enemy.transform.position, player.position) < enemy.aggroDistance)
            stateMachine.ChangeState(enemy.battleState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
