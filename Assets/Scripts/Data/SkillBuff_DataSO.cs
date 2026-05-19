using UnityEngine;

[CreateAssetMenu(fileName = "Skill buff - ", menuName = "RPG Setup/Skill/Skill Buff")]
public class SkillBuff_DataSO : Skill_BaseSO
{
    [Header("Skill Stat Buffs/Debuffs")]
    public bool isPercent = true;
    public BuffEffectData skillStatData;

    public override string GetUpgradeDescription()
    {
        string valueText = isPercent ? $"{skillStatData.value}%" : skillStatData.value.ToString();
        return $" + {valueText} {skillStatData.type}";
    }
}
