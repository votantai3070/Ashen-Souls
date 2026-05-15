public class EnemyRange_HitState : EnemyRange_State
{
    public EnemyRange_HitState(Enemy enemy, StateMachine<EntityState> stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = anim.GetCurrentAnimatorClipInfo(0).Length;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
        {
            stateMachine.ChangeState(enemyRange.idleState);
        }
    }
}
