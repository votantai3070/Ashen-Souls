using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill Enemy - ", menuName = "RPG Setup/Skill Enemy/Skill Enemy Data")]
public class SkillEnemy_DataSO : Skill_BaseSO
{
    [Header("Skill Enemy")]
    public LayerMask whatIsEnemy;
    public SkillEnemyData upgradeData;
    public GameObject skillObjectPrefab;

    [Serializable]
    public class SkillEnemyData
    {
        public SkillEnemyType enemyType;
        public float cooldown;
        public float distanceToAttack;
        public float attackRadius;
        public float speed;
    }
}
