using UnityEngine;

public class EnemyRange_ChaseState : EnemyRange_State
{
    public EnemyRange_ChaseState(Enemy enemy, StateMachine<EntityState> stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
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

        if (enemyRange.combat.CanSeePlayer() == false)
            stateMachine.ChangeState(enemyRange.moveState);

        Vector2 direction = enemyRange.GetDirectionPlayer();

        if (direction != Vector2.zero)
            enemyRange.Flip(direction);

        if (enemyRange.combat.CanAttack())
        {
            if (enemyRange.CanStopChaseRange())
                enemyRange.SetVelocity(0f, 0f);
            else
                enemyRange.SetVelocity(direction.x * enemyRange.chaseSpeed, direction.y * enemyRange.chaseSpeed);

            if (enemyRange.IsPlayerInAttackRange())
                stateMachine.ChangeState(enemyRange.attackState);
        }
    }
}
