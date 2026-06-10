//using System.Collections.Generic;
//using UnityEngine;

//public class Player_LevelMilestoneReward : MonoBehaviour
//{

//    private HashSet<int> claimedMilestones = new();
//    private Player_Stats playerStats;

//    private void Awake()
//    {
//        playerStats = GetComponent<Player_Stats>();
//    }

//    private void Start()
//    {
//        playerStats.levelSystem.OnLevelUp += HandleLevelUp;
//    }

//    private void OnDestroy()
//    {
//        if (playerStats != null)
//            playerStats.levelSystem.OnLevelUp -= HandleLevelUp;
//    }

//    private void HandleLevelUp(int newLevel)
//    {
//        if (!rewardMilestones.Contains(newLevel)) return;
//        if (claimedMilestones.Contains(newLevel)) return;

//        List<Skill_BaseSO> selectableSkills = UI.instance.skillBoardUI.GetMilestoneSelectableSkills();

//        if (selectableSkills == null || selectableSkills.Count == 0)
//            return;

//        claimedMilestones.Add(newLevel);
//        UI.instance.OpenMilestoneSkillBoard(selectableSkills);
//    }
//}