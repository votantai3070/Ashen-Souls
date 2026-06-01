using UnityEngine;

public class Player_SprintState : Player_GroundState
{
    public Player_SprintState(Player player, StateMachine<EntityState> stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        float animSpeed = player.sprintSpeed / player.moveSpeed;

        anim.SetFloat("MoveSpeed", animSpeed);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        float moveInputX = controls.MoveInput.x;
        float moveInputY = controls.MoveInput.y;

        anim.SetFloat("xMove", moveInputX);
        anim.SetFloat("yMove", moveInputY);

        if (stateMachine.currentState == player.dashState)
            player.SetVelocity(moveInputX * player.sprintSpeed, moveInputY * player.sprintSpeed);

        if (controls.inputActions.Player.Sprint.WasReleasedThisFrame())
        {
            if (controls.MoveInput != Vector2.zero)
                stateMachine.ChangeState(player.moveState);
            else
                stateMachine.ChangeState(player.idleState);

            return;
        }
    }
}
