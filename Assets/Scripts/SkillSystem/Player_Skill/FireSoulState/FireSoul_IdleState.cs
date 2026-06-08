public class FireSoul_IdleState : FireSoul_State
{
    public FireSoul_IdleState(SkillObject_FireSoul spellSkill, StateMachine<SpellState> stateMachine, string animBoolName) : base(spellSkill, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        rb.linearVelocity = new(0, 0);
        stateTimer = anim.GetCurrentAnimatorStateInfo(0).length;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer <= 0)
            stateMachine.ChangeState(spellSkill.shootAntecipationState);
    }
}
