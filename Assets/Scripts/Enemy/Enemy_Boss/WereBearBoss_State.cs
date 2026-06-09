public class WereBearBoss_State : EnemyBoss_State
{
    protected Boss_WereBear wereBear;
    public WereBearBoss_State(Enemy enemy, StateMachine<EntityState> stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        wereBear = enemy as Boss_WereBear;
    }
}
