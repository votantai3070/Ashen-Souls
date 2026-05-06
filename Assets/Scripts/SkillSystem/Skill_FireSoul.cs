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

    public override bool TryUseSkill()
    {
        if (base.TryUseSkill() == false)
            return false;

        if (fireSoul != null)
            return false;

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

        return true;
    }

    // Create fire soul
    public void CreateFireSoul(Vector3 scale)
    {
        fireSoul = ObjectPool.instance.Spawn(fireSoulGo.name, transform.position, transform.rotation);
        fireSoul.transform.localScale = scale;
        fireSoul.GetComponent<SkillObject_FireSoul>().SetupFireSoul(this, duration, whatIsEnemy);
    }

    public void OnSoulBurstExpired()
    {
        fireSoul = null;
    }

}
