using UnityEngine;

public class Skill_AbsorbSoul : Skill_Base
{
    [Header("Absorb Soul Settings")]
    public GameObject soul;
    private float distance = .5f;
    public float speedOfSoul = 5f;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void SetSkillUpgrade(Skill_DataSO skillData)
    {
        base.SetSkillUpgrade(skillData);

        soul = skillData.skillObjectPrefab;
    }

    public override void TryUseSkill()
    {
        if (!CanUseSkill())
            return;


        if (upgradeType == SkillUpgradeType.AbsorbSoulUpgrade)
        {
            speedOfSoul = 10f;

            SetSkillOnCooldown();
        }
    }

    public void AbsorbSoul(SkillObject_Soul soul)
    {
        if (Vector3.Distance(soul.transform.position, entity.transform.position) > distance)
            return;
    }
}
