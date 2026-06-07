using System.Collections.Generic;
using UnityEngine;

public class UI_SkillHolder : MonoBehaviour
{
    private UI_SkillSlot[] slots;
    [SerializeField] private List<Skill_Base> skillList = new List<Skill_Base>();

    private void Awake()
    {
        slots = GetComponentsInChildren<UI_SkillSlot>(true);
    }

    public void StartCooldownSkillSlot(SkillType skillType, float cooldown)
    {
        foreach (var slot in slots)
        {
            if (slot.skillData == null) continue;

            if (slot.skillData.skillType == skillType)
            {
                slot.StartCooldown(cooldown);
                return;
            }
        }
    }

    public void ResetCooldown(SkillType skillType)
    {
        foreach (var slot in slots)
        {
            if (slot.skillData == null) continue;

            if (slot.skillData.skillType == skillType)
            {
                slot.ResetCooldown();
                return;
            }
        }
    }

    public void SetupSkillSlots()
    {
        skillList.Clear();

        foreach (var skill in UI.instance.player.skillManager.allSkills)
        {
            if (skill.skillData != null)
                skillList.Add(skill);
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (i < skillList.Count)
            {
                slots[i].SetSkillSlot(skillList[i].skillData);
            }
            else
                slots[i].SetSkillSlot(null);
        }
    }
}
