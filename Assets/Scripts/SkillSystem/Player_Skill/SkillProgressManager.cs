using System.Collections.Generic;
using UnityEngine;

public class SkillProgressManager : MonoBehaviour
{
    public static SkillProgressManager instance { get; private set; }

    [SerializeField] private List<Skill_BaseSO> unlockedSkills = new();

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void UnlockSkill(Skill_BaseSO skill) => unlockedSkills.Add(skill);

    public bool IsUnlocked(Skill_BaseSO skill) => unlockedSkills.Contains(skill);

    public void Reset() => unlockedSkills.Clear();
}