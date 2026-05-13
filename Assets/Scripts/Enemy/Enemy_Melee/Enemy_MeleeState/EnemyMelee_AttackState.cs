public class EnemyMelee_AttackState : EnemyMelee_State
{
    public EnemyMelee_AttackState(Enemy enemy, StateMachine<EntityState> stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        enemyMelee.canTrigger = false;

        enemyMelee.SetVelocity(0f, 0f);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (enemyMelee.canTrigger)
        {
            stateMachine.ChangeState(enemyMelee.idleState);
        }
    }
}
