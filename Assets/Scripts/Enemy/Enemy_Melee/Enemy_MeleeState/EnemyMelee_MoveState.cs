using UnityEngine;

public class EnemyMelee_MoveState : EnemyMelee_GroundState
{
    private Vector2 direction;
    private Vector3 destination;

    public EnemyMelee_MoveState(Enemy enemy, StateMachine<EntityState> stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        //destination = enemyMelee.GetPatrolDestination();

        //GetDirection();
    }

    public override void Update()
    {
        base.Update();

        enemyMelee.SetVelocity(direction.x * enemyMelee.moveSpeed, direction.y * enemyMelee.moveSpeed);

        anim.SetFloat("xMove", Mathf.Round(direction.x));
        anim.SetFloat("yMove", Mathf.Round(direction.y));

        if (enemyMelee.CanStopChaseRange())
            stateMachine.ChangeState(enemyMelee.idleState);

    }

    public override void Exit()
    {
        base.Exit();
    }


}