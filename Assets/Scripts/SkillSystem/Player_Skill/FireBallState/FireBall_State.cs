public class FireBall_State : SpellState
{
    protected SkillObject_FireBall fireBall;

    public FireBall_State(SkillObject_FireBall spellSkill, StateMachine<SpellState> stateMachine, string animBoolName) : base(stateMachine, animBoolName)
    {
        fireBall = spellSkill;

        rb = spellSkill.rb;
        anim = spellSkill.anim;
    }
}
