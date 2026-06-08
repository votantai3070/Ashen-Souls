public class FireSould_ShootAntecipationState : FireSoul_State
{
    public FireSould_ShootAntecipationState(SkillObject_FireSoul spellSkill, StateMachine<SpellState> stateMachine, string animBoolName) : base(spellSkill, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = anim.GetCurrentAnimatorStateInfo(0).length;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            stateMachine.ChangeState(spellSkill.shotState);
    }
}
