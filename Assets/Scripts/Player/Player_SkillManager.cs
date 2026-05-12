using UnityEngine;

public class Player_SkillManager : MonoBehaviour
{
    private Player player;

    public Skill_AbsorbSoul absorbSoul { get; private set; }
    public Skill_FireSoul fireSoul { get; private set; }
    public Skill_SpinningSword spinningSword { get; private set; }
    public Skill_SoulBurst burst { get; private set; }
    public Skill_DeathDash deathDash { get; private set; }

    public Skill_Base[] allSkills { get; private set; }


    private void Awake()
    {
        player = GetComponent<Player>();

        absorbSoul = GetComponentInChildren<Skill_AbsorbSoul>();
        fireSoul = GetComponentInChildren<Skill_FireSoul>();
        spinningSword = GetComponentInChildren<Skill_SpinningSword>();
        burst = GetComponentInChildren<Skill_SoulBurst>();
        deathDash = GetComponentInChildren<Skill_DeathDash>();

        allSkills = GetComponentsInChildren<Skill_Base>();
    }

    private void Update()
    {
        foreach (var spell in allSkills)
        {
            if (spell.upgradeType == SkillUpgradeType.DeathDash ||
                spell.upgradeType == SkillUpgradeType.DeathDashUpgrade)
                return;

            if (spell.upgradeType != SkillUpgradeType.None)
                spell.TryUseSkill();
        }
    }

    public void ReduceAllSkillCooldownBy(float amount)
    {
        foreach (var skill in allSkills)
            skill.ReduceCooldownBy(amount);
    }

    public Skill_Base GetSkillByType(SkillType type)
    {
        switch (type)
        {
            case SkillType.AbsorbSoul: return absorbSoul;
            case SkillType.FireSoul: return fireSoul;
            case SkillType.SpinningSword: return spinningSword;
            case SkillType.SoulBurst: return burst;
            case SkillType.DeathDash: return deathDash;

            default:
                Debug.Log($"Skill type {type} is not implemented yet.");
                return null;
        }
    }

}
