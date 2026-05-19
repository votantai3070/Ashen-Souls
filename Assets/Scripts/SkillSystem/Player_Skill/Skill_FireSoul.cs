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

    public override void SetSkill(Skill_DataSO skillData)
    {
        base.SetSkill(skillData);

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
            CreateFireSoul();
            SetSkillOnCooldown();
        }

        return true;
    }

    // Create fire soul
    public void CreateFireSoul()
    {
        fireSoul = ObjectPool.instance.Spawn(fireSoulGo.name, transform.position, transform.rotation);
        fireSoul.GetComponent<SkillObject_FireSoul>().SetupFireSoul(this, duration, whatIsEnemy);
    }

    public void OnSoulBurstExpired()
    {
        fireSoul = null;
    }

}
