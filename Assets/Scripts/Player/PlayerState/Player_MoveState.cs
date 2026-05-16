using UnityEngine;

public class Player_MoveState : Player_GroundState
{
    public Player_MoveState(Player player, StateMachine<EntityState> stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();

        Vector2 moveInput = controls.moveInput;

        anim.SetFloat("xMove", moveInput.x);
        anim.SetFloat("yMove", moveInput.y);

        if (controls.moveInput != Vector2.zero)
        {
            player.SetValueIdleAndAttackAnimation(moveInput);
        }

        if (stateMachine.currentState != player.dashState)
            player.SetVelocity(moveInput.x * player.moveSpeed, moveInput.y * player.moveSpeed);

        if (controls.moveInput == Vector2.zero)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
