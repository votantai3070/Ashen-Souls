using UnityEngine;

public class EnemyMelee_DeadState : EnemyMelee_State
{
    public EnemyMelee_DeadState(Enemy enemy, StateMachine<EntityState> stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = anim.GetCurrentAnimatorStateInfo(0).length;

        enemyMelee.SetVelocity(0, 0);
        enemyMelee.rb.simulated = false;
        enemyMelee.GetComponent<Collider2D>().enabled = false;
    }

    public override void Exit()
    {
        base.Exit();

        enemyMelee.rb.simulated = true;
        enemyMelee.GetComponent<Collider2D>().enabled = true;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer <= 0f)
        {
            stateMachine.ChangeState(enemyMelee.idleState);
            enemyMelee.health.Die();
        }
    }

}
