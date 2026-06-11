using System.Collections.Generic;
using UnityEngine;

public class Skill_SpinningSword : Skill_Base
{
    public Stat swordCount;
    [Header("Spinning Sword")]
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private float orbitRadius = 1.5f;

    [SerializeField] private List<SkillObject_SpinningSword> activeSwords = new();

    protected override void Awake()
    {
        base.Awake();
    }

    public override void SetSkill(Skill_DataSO skillData)
    {
        base.SetSkill(skillData);

        swordCount.SetBaseValue(skillData.count);
        orbitRadius = skillData.radius;

        swordPrefab = skillData.skillObjectPrefab;
    }

    public override bool TryUseSkill()
    {
        if (base.TryUseSkill() == false)
            return false;

        if (activeSwords.Count > 0)
            return false;

        if (CheckEnemyRadius())
        {
            SpawnSwords(swordCount.GetValue());
            SetSkillOnCooldown();
        }

        return true;
    }

    private void SpawnSwords(float swordCount = 2)
    {
        float angleStep = 360f / swordCount;

        for (int i = 0; i < swordCount; i++)
        {
            float startAngle = angleStep * i;

            GameObject obj = ObjectPool.instance.Spawn(swordPrefab.name, transform.position, Quaternion.identity);
            SkillObject_SpinningSword sword = obj.GetComponent<SkillObject_SpinningSword>();
            sword.SetupSword(this, entity, orbitRadius, duration, whatIsEnemy, startAngle);

            activeSwords.Add(sword);
        }
    }

    protected override Stat GetStat(StatType upgradeType)
    {
        return upgradeType switch
        {
            StatType.Damage => damageSkill,
            StatType.Speed => speedSkill,
            StatType.Size => sizeSkill,
            StatType.AttackSpeed => attackSpeedSkill,
            StatType.Count => swordCount,
            _ => null
        };
    }

    // Call from SkillObject when duration expires
    public void OnSwordExpired(SkillObject_SpinningSword sword)
    {
        activeSwords.Remove(sword);
    }
}