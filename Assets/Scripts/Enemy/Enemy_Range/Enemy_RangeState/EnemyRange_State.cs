public class EnemyRange_State : EnemyState
{
    protected Enemy_Range enemyRange;

    public EnemyRange_State(Enemy enemy, StateMachine<EntityState> stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        enemyRange = enemy as Enemy_Range;
    }
}
