using UnityEngine;

public class WereBear_AttackState : WereBearBoss_State
{
    private int currentAttack;
    private int maxAttacks = 3;

    public WereBear_AttackState(Enemy enemy, StateMachine<EntityState> stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        wereBear.canTrigger = false;
        wereBear.SetVelocity(0f, 0f);
        wereBear.SetAnimIdleAndAttackAnimation();

        currentAttack = GetRandomAttack();
        anim.SetInteger("AttackCounter", currentAttack);
    }

    public override void Exit()
    {
        base.Exit();

        if (currentAttack == maxAttacks)
            wereBear.skillManager.GetSkillEnemyByType(SkillEnemyType.WerebearShockwave).TryUseSkill();
    }

    public override void Update()
    {
        base.Update();

        if (currentAttack != 3)
            wereBear.combat.PerformAttack(wereBear);

        if (wereBear.canTrigger)
        {
            if (wereBear.combat.CanAttack() == false)
                stateMachine.ChangeState(wereBear.chaseState);
            else
                stateMachine.ChangeState(wereBear.idleState);
        }
    }

    private int GetRandomAttack()
    {
        bool hasShockwave = wereBear.skillManager
            .GetSkillEnemyByType(SkillEnemyType.WerebearShockwave).skillObject == null;

        return hasShockwave
                ? Random.Range(1, maxAttacks + 1)
                : Random.Range(1, maxAttacks);
    }
}
