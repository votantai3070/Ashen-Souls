public class EnemyMelee_IdleState : EnemyMelee_GroundState
{
    public EnemyMelee_IdleState(Enemy enemy, StateMachine<EntityState> stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = enemyMelee.idleTimer;

        enemyMelee.SetVelocity(0f, 0f);
        enemyMelee.IdleAndAttackAnimation();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        //if (stateTimer < 0)
        //    stateMachine.ChangeState(enemy.moveState);
    }
}
