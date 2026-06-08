public class FireSoul_State : SpellState
{
    protected SkillObject_FireSoul spellSkill;
    public FireSoul_State(SkillObject_FireSoul spellSkill, StateMachine<SpellState> stateMachine, string animBoolName) : base(stateMachine, animBoolName)
    {
        this.spellSkill = spellSkill;

        rb = spellSkill.rb;
        anim = spellSkill.anim;
    }
}
