using UnityEngine;

public class SkillBoss_WerebearShockwave : SkillEnemy_Base
{
    public override bool TryUseSkill()
    {
        if (base.TryUseSkill() == false)
            return false;

        Debug.Log("Use Skill");
        CreateSkillObject();
        SetOnCooldown();

        return true;
    }
}
