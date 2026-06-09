using UnityEngine;

public class WereBear_IdleState : WereBearBoss_State
{
    public WereBear_IdleState(Enemy enemy, StateMachine<EntityState> stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = wereBear.idleTimer;

        wereBear.SetVelocity(0f, 0f);
        wereBear.SetAnimIdleAndAttackAnimation();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        Debug.Log("CanSeePlayer: " + wereBear.combat.CanSeePlayer());

        if (wereBear.combat.CanSeePlayer() && stateTimer < 0)
            stateMachine.ChangeState(wereBear.chaseState);
    }
}
