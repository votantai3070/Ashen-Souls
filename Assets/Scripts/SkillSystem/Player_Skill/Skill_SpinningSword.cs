using System.Collections.Generic;
using UnityEngine;

public class Skill_SpinningSword : Skill_Base
{
    [Header("Spinning Sword")]
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private float orbitRadius = 1.5f;
    [SerializeField] private float orbitSpeed = 180f;
    [SerializeField] private int swordCount = 3;

    [SerializeField] private List<SkillObject_SpinningSword> activeSwords = new();

    protected override void Awake()
    {
        base.Awake();
    }

    public override void SetSkill(Skill_DataSO skillData)
    {
        base.SetSkill(skillData);

        orbitRadius = skillData.orbitRadius;
        orbitSpeed = skillData.orbitSpeed;
        swordCount = skillData.swordCount;
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
            SpawnSwords(swordCount);
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
            sword.SetupSword(this, entity, orbitRadius, orbitSpeed, duration, startAngle);

            activeSwords.Add(sword);
        }
    }

    // Call from SkillObject when duration expires
    public void OnSwordExpired(SkillObject_SpinningSword sword)
    {
        activeSwords.Remove(sword);
    }
}