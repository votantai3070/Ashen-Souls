public class EnemyState : EntityState
{
    public EnemyState(Enemy enemy, StateMachine<EntityState> stateMachine, string animBoolName) : base(stateMachine, animBoolName)
    {
        anim = enemy.anim;
        rb = enemy.rb;
    }

}
