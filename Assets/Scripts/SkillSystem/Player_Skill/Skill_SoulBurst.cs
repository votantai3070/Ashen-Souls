using UnityEngine;

public class Skill_SoulBurst : Skill_Base
{
    [SerializeField] private GameObject soulExplodeGo;

    private GameObject soulExplosion;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void SetSkill(Skill_DataSO skillData)
    {
        base.SetSkill(skillData);

        soulExplodeGo = skillData.skillObjectPrefab;
    }

    public override bool TryUseSkill()
    {
        if (base.TryUseSkill() == false)
            return false;

        if (soulExplosion != null)
            return false;

        if (CheckEnemyRadius())
        {
            CreateSoulExplode();
            SetSkillOnCooldown();
        }

        return true;
    }

    private void CreateSoulExplode()
    {
        soulExplosion = ObjectPool.instance.Spawn(soulExplodeGo.name, transform.position, Quaternion.identity);
        soulExplosion.GetComponent<SkillObject_SoulBurst>().SetSoulBurst(this, whatIsEnemy);
    }

    public void OnSoulBurstExpired()
    {
        soulExplosion = null;
    }
}
