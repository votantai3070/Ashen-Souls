using UnityEngine;

public class Player_DashState : PlayerState
{
    public Player_DashState(Player player, StateMachine<EntityState> stateMachine, string animBoolName)
        : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        player.combat.SetInvulnerable(true);
        stateTimer = skillManager.deathDash.GetDuration();
        player.skillManager.deathDash.TryUseSkill();

        player.SetVelocity(player.dashSpeed * controls.moveInput.x, player.dashSpeed * controls.moveInput.y);
    }

    public override void Exit()
    {
        base.Exit();

        player.combat.SetInvulnerable(false);
        player.SetVelocity(0, 0);
    }

    public override void Update()
    {
        base.Update();


        if (stateTimer < 0)
            if (controls.moveInput != Vector2.zero)
                stateMachine.ChangeState(player.moveState);
            else
                stateMachine.ChangeState(player.idleState);
    }
}