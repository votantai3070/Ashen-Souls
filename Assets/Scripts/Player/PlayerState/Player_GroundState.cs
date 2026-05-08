public class Player_GroundState : PlayerState
{
    public Player_GroundState(Player player, StateMachine<EntityState> stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();

        // Transition to Attack State
        if (controls.PressedAttack())
        {
            stateMachine.ChangeState(player.attackState);
        }

    }
}
