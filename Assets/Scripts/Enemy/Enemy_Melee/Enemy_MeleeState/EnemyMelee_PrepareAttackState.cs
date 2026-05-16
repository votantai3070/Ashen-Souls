public class EnemyMelee_PrepareAttackState : EnemyMelee_State
{
    public EnemyMelee_PrepareAttackState(Enemy enemy, StateMachine<EntityState> stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = anim.GetCurrentAnimatorClipInfo(0).Length;
        enemyMelee.SetVelocity(0, 0);
        enemyMelee.SetAnimIdleAndAttackAnimation();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer <= 0)
            stateMachine.ChangeState(enemyMelee.attackState);
    }
}
