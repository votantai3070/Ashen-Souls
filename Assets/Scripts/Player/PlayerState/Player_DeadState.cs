public class Player_DeadState : PlayerState
{
    public Player_DeadState(Player player, StateMachine<EntityState> stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = anim.GetCurrentAnimatorStateInfo(0).length;
        player.health.DurationDied = stateTimer;
    }
}
