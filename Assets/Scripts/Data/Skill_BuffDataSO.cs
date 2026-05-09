using UnityEngine;

[CreateAssetMenu(fileName = "Skill buff - ", menuName = "RPG Setup/Skill/Skill Buff")]
public class Skill_BuffDataSO : Skill_BaseSO
{
    [Header("Skill Stat Buffs/Debuffs")]
    public bool isPercent = true;
    public BuffEffectData[] skillStatData;
}
