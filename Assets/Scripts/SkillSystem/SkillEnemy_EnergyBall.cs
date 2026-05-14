using UnityEngine;

public class SkillEnemy_EnergyBall : SkillEnemy_Base
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
