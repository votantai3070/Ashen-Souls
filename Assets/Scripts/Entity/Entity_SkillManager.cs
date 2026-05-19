using UnityEngine;

public class Entity_SkillManager : MonoBehaviour
{
    public Skill_Base[] allSkills { get; private set; }

    protected virtual void Awake()
    {
        allSkills = GetComponentsInChildren<Skill_Base>();
    }

    protected virtual void Update()
    {
        TryUseSkillByType();
    }

    public void TryUseSkillByType()
    {
        if (allSkills.Length == 0) return;

        foreach (var spell in allSkills)
        {
            if (spell.skillType == SkillType.DeathDash)
                return;

            if (spell.skillType != SkillType.None)
                spell.TryUseSkill();
        }
    }

    public void ReduceAllSkillCooldownBy(float amount)
    {
        foreach (var skill in allSkills)
            skill.ReduceCooldownBy(amount);
    }
}
