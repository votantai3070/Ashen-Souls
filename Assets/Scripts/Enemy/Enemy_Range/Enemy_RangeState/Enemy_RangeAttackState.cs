public class Enemy_RangeAttackState : Enemy_RangeState
{
    public Enemy_RangeAttackState(Enemy enemy, StateMachine<EntityState> stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
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
