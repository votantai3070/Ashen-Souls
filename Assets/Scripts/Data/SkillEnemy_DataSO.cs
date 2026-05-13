using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill Enemy - ", menuName = "RPG Setup/Skill Enemy/Skill Enemy Data")]
public class SkillEnemy_DataSO : Skill_BaseSO
{
    [Header("Skill Enemy")]
    public LayerMask whatIsEnemy;
    public UpgradeData upgradeData;
    public GameObject skillObjectPrefab;

    [Serializable]
    public class UpgradeData
    {
        public SkillEnemyType enemyType;
        public float cooldown;
        public float distanceToAttack;
        public float attackRadius;
        public float speed;
    }
}
