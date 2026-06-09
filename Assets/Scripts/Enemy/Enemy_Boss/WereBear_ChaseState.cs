using UnityEngine;

public class WereBear_ChaseState : WereBearBoss_State
{
    public WereBear_ChaseState(Enemy enemy, StateMachine<EntityState> stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();

        float animSpeed = wereBear.chaseSpeed / wereBear.baseChaseSpeed;
        anim.SetFloat("ChaseSpeed", animSpeed);
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();

        //if (wereBear.combat.CanSeePlayer() == false)
        //    stateMachine.ChangeState(wereBear.moveState);

        Vector2 dir = wereBear.GetDirectionPlayer();
        wereBear.MovementAnimation(dir);

        wereBear.SetValueIdleAndAttackAnimation(dir);

        if (wereBear.combat.CanAttack() == false)
        {
            if (Vector2.Distance(wereBear.transform.position, wereBear.player.position) <= wereBear.chaseStopDistance)
                wereBear.SetVelocity(0f, 0f);
            else
                wereBear.SetVelocity(dir.x * wereBear.chaseSpeed, dir.y * wereBear.chaseSpeed);
        }

        else if (wereBear.combat.CanAttack())
        {
            wereBear.SetVelocity(dir.x * wereBear.chaseSpeed, dir.y * wereBear.chaseSpeed);
            if (wereBear.IsPlayerInAttackRange())
            {
                stateMachine.ChangeState(wereBear.attackState);
            }
        }
    }
}