using UnityEngine;

public class EnemyRange_MoveState : EnemyRange_State
{
    public EnemyRange_MoveState(Enemy enemy, StateMachine<EntityState> stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        float animSpeed = enemyRange.chaseSpeed / enemyRange.moveSpeed;
        anim.SetFloat("ChaseSpeed", animSpeed);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        Vector2 direction = enemyRange.GetDirectionPlayer();

        enemyRange.SetVelocity(direction.x * enemyRange.moveSpeed, direction.y * enemyRange.moveSpeed);

        anim.SetFloat("xMove", Mathf.Round(direction.x));
        anim.SetFloat("yMove", Mathf.Round(direction.y));

        if (enemyRange.CanStopChaseRange())
            stateMachine.ChangeState(enemyRange.idleState);
    }
}
