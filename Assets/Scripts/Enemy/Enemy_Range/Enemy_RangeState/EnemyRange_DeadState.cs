using UnityEngine;

public class EnemyRange_DeadState : EnemyRange_State
{
    public EnemyRange_DeadState(Enemy enemy, StateMachine<EntityState> stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = anim.GetCurrentAnimatorStateInfo(0).length;

        enemyRange.SetVelocity(0, 0);
        enemyRange.rb.simulated = false;
        enemyRange.GetComponent<Collider2D>().enabled = false;
    }

    public override void Exit()
    {
        base.Exit();

        enemyRange.rb.simulated = true;
        enemyRange.GetComponent<Collider2D>().enabled = true;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer <= 0f)
        {
            stateMachine.ChangeState(enemyRange.idleState);
            enemyRange.health.Die();
        }
    }
}
