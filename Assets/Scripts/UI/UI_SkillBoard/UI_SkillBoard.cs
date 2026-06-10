using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UI_SkillBoard : MonoBehaviour
{
    [SerializeField] private Skill_ListDataSO skillsData;
    [SerializeField] private UI_SkillCard[] cards;

    List<Skill_BaseSO> availableSkills;

    [Header("Card restrictions")]
    [SerializeField] private int maxRarityAmount = 1200;
    [SerializeField] private int maxSkillToRoll = 3;

    private void Awake()
    {
        cards = GetComponentsInChildren<UI_SkillCard>();
    }

    private void OnEnable()
    {
        GetSkillsRollRandom();
    }

    public void GetSkillsRollRandom()
    {
        List<Skill_BaseSO> skillsToRoll = RollSkills();
        int amountSkills = Mathf.Min(skillsToRoll.Count, maxSkillToRoll);

        for (int i = 0; i < cards.Length; i++)
        {
            if (i < amountSkills)
            {
                string color = GetColorByRarity(skillsToRoll[i].skillRarity);
                cards[i].SetCardInfo(skillsToRoll[i], color);
                cards[i].ResetCard();
                cards[i].gameObject.SetActive(true);
            }
            else
                cards[i].gameObject.SetActive(false);
        }
    }

    public List<Skill_BaseSO> RollSkills()
    {
        List<Skill_BaseSO> possibleSkills = new();
        List<Skill_BaseSO> finalSkills = new();
        List<Skill_BaseSO> availiableSkills = InitializeSkills();

        float maxRarityAmount = this.maxRarityAmount;

        // Step 1: Roll each item based on rarity and max drop chance
        foreach (var skill in availiableSkills)
        {
            float rollChance = skill.GetSkillRollChance();

            bool isMilestoneSkill = skill.skillMilestoneType == SkillMilestoneType.SkillMilestone;

            if (isMilestoneSkill || Random.Range(0, 100) < rollChance)
                possibleSkills.Add(skill);
        }

        //Step 2: Sort by rarity (hightest to lowest)
        possibleSkills = possibleSkills.OrderByDescending(skill => skill.skillRarity).ToList();

        //Step 3: Add items to final drop list until rarity limit in entity is reached
        foreach (var skill in possibleSkills)
        {
            if (maxRarityAmount > skill.skillRarity)
            {
                finalSkills.Add(skill);
                maxRarityAmount -= skill.skillRarity;
            }
        }

        return finalSkills;
    }

    private List<Skill_BaseSO> InitializeSkills()
    {
        availableSkills = new();
        var progress = SkillProgressManager.instance;
        bool isMilestoneReward = LevelManager.instance.CanRewardMilestone();

        foreach (var skill in skillsData.skillList)
        {
            if (skill == null) continue;
            if (progress.IsUnlocked(skill)) continue;

            if (!isMilestoneReward && skill.skillMilestoneType == SkillMilestoneType.SkillMilestone)
                continue;

            if (isMilestoneReward && skill.skillMilestoneType != SkillMilestoneType.SkillMilestone)
                continue;

            if (skill.prerequisiteSkill == null)
            {
                availableSkills.Add(skill);
                continue;
            }

            if (progress.IsUnlocked(skill.prerequisiteSkill))
            {
                availableSkills.Add(skill);
            }
        }

        return availableSkills;
    }

    private string GetColorByRarity(int rarity)
    {
        if (rarity <= 200) return GameColors.Common; // Common
        if (rarity <= 400) return GameColors.Uncommon; // Uncommon
        if (rarity <= 600) return GameColors.Rare; // Rare
        if (rarity <= 900) return GameColors.Epic; // Epic
        if (rarity < 1000) return GameColors.Legendary;  // Legendary
        return GameColors.SkillCardColor; // Skill Card     
    }
}
