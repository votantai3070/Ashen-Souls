using UnityEngine;

public class Player_SkillManager : Entity_SkillManager
{
    private Player player;

    public Skill_AbsorbSoul absorbSoul { get; private set; }
    public Skill_FireSoul fireSoul { get; private set; }
    public Skill_SpinningSword spinningSword { get; private set; }
    public Skill_SoulBurst burst { get; private set; }
    public Skill_DeathDash deathDash { get; private set; }
    public Skill_SlashSword slashSword { get; private set; }

    public Skill_DataSO[] skills;


    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<Player>();

        absorbSoul = GetComponentInChildren<Skill_AbsorbSoul>();
        fireSoul = GetComponentInChildren<Skill_FireSoul>();
        spinningSword = GetComponentInChildren<Skill_SpinningSword>();
        burst = GetComponentInChildren<Skill_SoulBurst>();
        deathDash = GetComponentInChildren<Skill_DeathDash>();
        slashSword = GetComponentInChildren<Skill_SlashSword>();
    }

    private void Start()
    {
        foreach (var skillData in skills)
        {
            GetSkillByType(skillData.skillType)?.SetSkill(skillData);
            SkillProgressManager.instance.UnlockSkill(skillData);
        }
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
            case SkillType.SoulSlashSword: return slashSword;

            default:
                Debug.Log($"Skill type {type} is not implemented yet.");
                return null;
        }
    }
}
