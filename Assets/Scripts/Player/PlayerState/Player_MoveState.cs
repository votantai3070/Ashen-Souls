using UnityEngine;

public class Player_MoveState : Player_GroundState
{
    public Player_MoveState(Player player, StateMachine<EntityState> stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();

        float moveInputX = controls.moveInput.x;
        float moveInputY = controls.moveInput.y;

        anim.SetFloat("xMove", moveInputX);
        anim.SetFloat("yMove", moveInputY);

        if (controls.moveInput != Vector2.zero)
        {
            player.xIdleAndAttack = moveInputX;
            player.yIdleAndAttack = moveInputY;
        }

        if (stateMachine.currentState != player.dashState)
            player.SetVelocity(moveInputX * player.moveSpeed, moveInputY * player.moveSpeed);

        if (controls.moveInput == Vector2.zero)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
