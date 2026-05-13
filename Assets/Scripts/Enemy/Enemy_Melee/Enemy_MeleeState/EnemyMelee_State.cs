public class EnemyMelee_State : EnemyState
{
    protected Enemy_Melee enemyMelee;

    public EnemyMelee_State(Enemy enemy, StateMachine<EntityState> stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        enemyMelee = enemy as Enemy_Melee;
    }
}
