public class Enemy_RangeState : EnemyState
{
    protected Enemy_Range enemyRange;

    public Enemy_RangeState(Enemy enemy, StateMachine<EntityState> stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        enemyRange = enemy as Enemy_Range;
    }
}
