
using UnityEngine;

public class EnemyMelee_ChaseState : EnemyMelee_GroundState
{
    public EnemyMelee_ChaseState(Enemy enemy, StateMachine<EntityState> stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        float animSpeed = enemyMelee.chaseSpeed / enemyMelee.moveSpeed;
        anim.SetFloat("ChaseSpeed", animSpeed);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (enemyMelee.combat.CanSeePlayer() == false)
            stateMachine.ChangeState(enemyMelee.moveState);

        Vector2 dir = enemyMelee.GetDirectionPlayer();
        enemyMelee.MovementAnimation(dir);

        enemyMelee.SetValueIdleAndAttackAnimation(dir);

        if (enemyMelee.combat.CanAttack() == false)
        {
            if (Vector2.Distance(enemyMelee.transform.position, enemyMelee.player.position) <= enemyMelee.chaseStopDistance)
                enemyMelee.SetVelocity(0f, 0f);
            else
                enemyMelee.SetVelocity(dir.x * enemyMelee.chaseSpeed, dir.y * enemyMelee.chaseSpeed);
        }

        else if (enemyMelee.combat.CanAttack())
        {
            enemyMelee.SetVelocity(dir.x * enemyMelee.chaseSpeed, dir.y * enemyMelee.chaseSpeed);
            if (enemyMelee.IsPlayerInAttackRange())
            {
                if (enemyMelee.enemyType == EnemyMeleeType.Normal)
                    stateMachine.ChangeState(enemyMelee.attackState);
                else if (enemyMelee.enemyType == EnemyMeleeType.Special)
                    stateMachine.ChangeState(enemyMelee.prepareAttackState);
            }
        }
    }
}
