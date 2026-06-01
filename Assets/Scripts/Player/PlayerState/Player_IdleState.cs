using UnityEngine;

public class Player_IdleState : Player_GroundState
{
    public Player_IdleState(Player player, StateMachine<EntityState> stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        player.SetAnimIdleAndAttackAnimation();
    }

    public override void Update()
    {
        base.Update();

        if (controls.MoveInput != Vector2.zero)
        {
            stateMachine.ChangeState(player.moveState);
        }
    }
}
