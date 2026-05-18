using UnityEngine;

[CreateAssetMenu(fileName = "Skill - ", menuName = "RPG Setup/Skill/Skill Data")]
public class Skill_DataSO : Skill_BaseSO
{
    public GameObject skillObjectPrefab;

    [Header("Spinning Sword Skill")]
    public float orbitRadius = 1.5f;
    public float orbitSpeed = 180f;
    public int swordCount = 3;

    [Header("Upgrade")]
    public SkillType skillType;
    [Range(0, 100)]
    public float upgradeBoostChance = 30f;
    public float attackCooldownGuard = .5f;
    public UpgradeData upgradeData;


    [System.Serializable]
    public class UpgradeData
    {
        public SkillUpgradeType upgradeType;
        public float cooldown;
        public float distanceToAttack;
        public float attackRadius;
        public float speed;
        //public DamageScaleData damageScale;
    }


}
