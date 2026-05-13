public class EnemyMelee_GroundState : EnemyMelee_State
{
    public EnemyMelee_GroundState(Enemy enemy, StateMachine<EntityState> stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();

        if (enemyMelee.combat.CanSeePlayer() && stateTimer < 0)
            stateMachine.ChangeState(enemyMelee.chaseState);
    }
}
