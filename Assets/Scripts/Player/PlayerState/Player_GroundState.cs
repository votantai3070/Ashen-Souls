using UnityEngine;

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

        // Transition to Move State
        if (player.isSprinting == false && controls.moveInput != Vector2.zero)
        {
            stateMachine.ChangeState(player.moveState);
        }

        #region Dash & Sprint
        if (controls.inputActions.Player.Dash.WasPressedThisFrame())
        {
            Debug.Log("Dash input nhận được!");
            Debug.Log("Current state: " + stateMachine.currentState);
            stateMachine.ChangeState(player.dashState);
            Debug.Log("After change: " + stateMachine.currentState);
        }


        if (controls.inputActions.Player.Sprint.IsPressed())
        {
            stateMachine.ChangeState(player.sprintState);
        }
        #endregion
    }
}
