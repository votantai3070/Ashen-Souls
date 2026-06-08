using UnityEngine;

public class Skill_FireBall : Skill_Base
{
    [Header("Fire Ball Config")]
    [SerializeField] private GameObject fireBallGo;

    private GameObject fireBall;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void SetSkill(Skill_DataSO skillData)
    {
        base.SetSkill(skillData);

        fireBallGo = skillData.skillObjectPrefab;
    }

    public override bool TryUseSkill()
    {
        if (base.TryUseSkill() == false)
            return false;

        if (fireBall != null)
            return false;

        if (CheckEnemyRadius())
        {
            CreateFireBall();
            SetSkillOnCooldown();
        }

        return true;
    }

    // Create fire ball
    public void CreateFireBall()
    {
        fireBall = ObjectPool.instance.Spawn(fireBallGo.name, transform.position, transform.rotation);
        fireBall.GetComponent<SkillObject_FireBall>().SetupFireBall(this, duration, whatIsEnemy);
    }

    public void OnBallBurstExpired()
    {
        fireBall = null;
    }
}
