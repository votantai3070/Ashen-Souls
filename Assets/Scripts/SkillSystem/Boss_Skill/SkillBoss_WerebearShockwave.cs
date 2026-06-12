public class SkillBoss_WerebearShockwave : SkillEnemy_Base
{
    public override bool TryUseSkill()
    {
        if (base.TryUseSkill() == false)
            return false;

        CreateSkillObject();
        SetOnCooldown();

        return true;
    }
}
