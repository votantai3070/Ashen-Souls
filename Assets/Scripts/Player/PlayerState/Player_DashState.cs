using UnityEngine;

public class Player_DashState : Player_GroundState
{
    public Player_DashState(Player player, StateMachine<EntityState> stateMachine, string animBoolName)
        : base(player, stateMachine, animBoolName) { }

    public override void Enter()
    {
        base.Enter();

        player.SetVelocity(player.dashSpeed * controls.moveInput.x, player.dashSpeed * controls.moveInput.y);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        if (controls.moveInput != Vector2.zero)
            stateMachine.ChangeState(player.moveState);
        else
            stateMachine.ChangeState(player.idleState);
    }
}