using UnityEngine;

public class Skill_SoulBurst : Skill_Base
{
    [SerializeField] private GameObject soulExplodeGo;

    private GameObject soulExplosionGo;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void SetSkillUpgrade(Skill_DataSO skillData)
    {
        base.SetSkillUpgrade(skillData);

        soulExplodeGo = skillData.skillObjectPrefab;
    }

    public override void TryUseSkill()
    {
        base.TryUseSkill();

        if (soulExplosionGo != null)
            return;

        if (CheckEnemyRadius())
        {
            if (upgradeType == SkillUpgradeType.SoulBurst)
            {
                CreateSoulExplode();
                SetSkillOnCooldown();
            }
        }
    }

    private void CreateSoulExplode()
    {
        soulExplosionGo = ObjectPool.instance.Spawn(soulExplodeGo.name, transform.position, Quaternion.identity);
        soulExplosionGo.GetComponent<SkillObject_SoulBurst>().SetSoulBurst(this, duration, whatIsEnemy);
    }

    public void OnSoulBurstExpired()
    {
        soulExplosionGo = null;
    }
}
