public class EnemyRange_IdleState : EnemyRange_State
{
    public EnemyRange_IdleState(Enemy enemy, StateMachine<EntityState> stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = enemyRange.idleTimer;

        enemyRange.SetVelocity(0f, 0f);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (enemyRange.combat.CanSeePlayer() && stateTimer < 0)
            stateMachine.ChangeState(enemyRange.chaseState);
    }
}
