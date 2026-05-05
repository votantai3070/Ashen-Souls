using UnityEngine;

public class Skill_FireSoul : Skill_Base
{
    [Header("Fire Soul Ball Config")]
    [SerializeField] private GameObject fireSoulGo;

    private GameObject fireSoul;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void SetSkillUpgrade(Skill_DataSO skillData)
    {
        base.SetSkillUpgrade(skillData);

        fireSoulGo = skillData.skillObjectPrefab;
    }

    public override void TryUseSkill()
    {
        base.TryUseSkill();

        if (fireSoul != null && upgradeType == SkillUpgradeType.FireSoul)
            return;

        if (CheckEnemyRadius())
        {
            if (upgradeType == SkillUpgradeType.FireSoul)
            {
                CreateFireSoul(Vector3.one * 3);
                SetSkillOnCooldown();
            }
            else if (upgradeType == SkillUpgradeType.FireSoulUpgrade)
            {
                CreateFireSoul(Vector3.one * 5);
                SetSkillOnCooldown();
            }
        }
    }

    // Create fire soul
    public void CreateFireSoul(Vector3 scale)
    {
        fireSoul = ObjectPool.instance.Spawn(fireSoulGo.name, transform.position, transform.rotation);
        fireSoul.transform.localScale = scale;
        fireSoul.GetComponent<SkillObject_FireSoul>().SetupFireSoul(this, whatIsEnemy);
    }

    public void OnSoulBurstExpired()
    {
        fireSoul = null;
    }

}
