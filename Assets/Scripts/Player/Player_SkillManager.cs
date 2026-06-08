using UnityEngine;

public class Player_SkillManager : Entity_SkillManager, ISaveable
{
    private Player player;

    public Skill_AbsorbSoul absorbSoul { get; private set; }
    public Skill_FireSoul fireSoul { get; private set; }
    public Skill_SpinningSword spinningSword { get; private set; }
    public Skill_SoulBurst burst { get; private set; }
    public Skill_DeathDash deathDash { get; private set; }
    public Skill_SlashSword slashSword { get; private set; }
    public Skill_FireBall fireBall { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        player = GetComponentInParent<Player>();

        absorbSoul = GetComponentInChildren<Skill_AbsorbSoul>();
        fireSoul = GetComponentInChildren<Skill_FireSoul>();
        spinningSword = GetComponentInChildren<Skill_SpinningSword>();
        burst = GetComponentInChildren<Skill_SoulBurst>();
        deathDash = GetComponentInChildren<Skill_DeathDash>();
        slashSword = GetComponentInChildren<Skill_SlashSword>();
        fireBall = GetComponentInChildren<Skill_FireBall>();
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
            case SkillType.FireBall: return fireBall;

            default:
                Debug.Log($"Skill type {type} is not implemented yet.");
                return null;
        }
    }

    public void LoadData(GameData data)
    {
        if (!string.IsNullOrEmpty(data.selectedCharacterId))
        {
            PlayerDataSO character = player.characterList.GetCharacterData(data.selectedCharacterId);
            if (character == null)
            {
                Debug.LogWarning($"Character with ID {data.selectedCharacterId} not found.");
                return;
            }

            if (character.skillData == null)
            {
                Debug.LogWarning($"Character {character.characterName} has null skillData.");
                return;
            }

            Skill_Base skill = GetSkillByType(character.skillData.skillType);
            if (skill == null)
            {
                Debug.LogWarning($"No Skill_Base found for {character.skillData.skillType}.");
                return;
            }

            skill.SetSkill(character.skillData);

            if (SkillProgressManager.instance != null)
                SkillProgressManager.instance.UnlockSkill(character.skillData);
        }
    }

    public void SaveData(ref GameData data)
    {
    }
}
