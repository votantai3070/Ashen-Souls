public class EnemyRange_AttackState : EnemyRange_State
{
    public EnemyRange_AttackState(Enemy enemy, StateMachine<EntityState> stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        enemyRange.canTrigger = false;

        enemyRange.SetVelocity(0f, 0f);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (enemyRange.canTrigger)
        {
            stateMachine.ChangeState(enemyRange.idleState);
        }
    }
}
