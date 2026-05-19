using UnityEditor;
using UnityEngine;

public class Skill_BaseSO : ScriptableObject
{
    public string saveId;

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

    private void OnValidate()
    {
        skillRollChance = GetSkillRollChance();

#if UNITY_EDITOR
        string path = AssetDatabase.GetAssetPath(this);
        saveId = AssetDatabase.AssetPathToGUID(path);
#endif  
    }

    public float GetSkillRollChance()
    {
        float maxRarity = 1000;
        float chance = (maxRarity - skillRarity + 1) / maxRarity * 100;

        return Mathf.Min(chance, maxSkillRollChance);
    }
}
