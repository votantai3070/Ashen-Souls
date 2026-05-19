using UnityEditor;
using UnityEngine;

public class Skill_BaseSO : ScriptableObject
{
    public string saveId;
    public string skillId;

    [Header("Skill Description")]
    public string displayName;
    [TextArea(3, 10)]
    public string description;
    public Sprite icon;
    public float duration = 5f;
    [Space]
    public Skill_BaseSO prerequisiteSkill;

    [Header("Skill Card")]
    public SkillType skillType;
    public CardType cardType;

    [Space]
    [Range(0, 1000)]
    public int skillRarity = 100;
    [Range(0, 100)]
    public float skillRollChance;
    [Range(0, 100)]
    public float maxSkillRollChance = 65f;

    protected virtual void OnValidate()
    {
        skillRollChance = GetSkillRollChance();

#if UNITY_EDITOR
        saveId = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(this));
        skillId = saveId;
#endif
    }

    public float GetSkillRollChance()
    {
        float maxRarity = 1000;
        float chance = (maxRarity - skillRarity + 1) / maxRarity * 100;

        return Mathf.Min(chance, maxSkillRollChance);
    }

    public virtual string GetUpgradeDescription()
    {
        return string.Empty;
    }
}