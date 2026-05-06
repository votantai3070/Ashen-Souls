using UnityEngine;
using static Skill_DataSO;

public class Skill_Base : MonoBehaviour
{
    public Player_SkillManager skillManager { get; private set; }
    public Entity entity { get; private set; }

    [Space]
    public LayerMask whatIsEnemy;
    public Transform target;
    [SerializeField] protected Transform targetCheck;

    [Header("General details")]
    [SerializeField] protected SkillType skillType;
    public SkillUpgradeType upgradeType;
    [SerializeField] protected float cooldown;
    public float speed;
    public float checkEnemyRadius;
    public float checkDamageRadius;
    private float lastTimeUsed;

    [SerializeField] protected float duration = 5f;

    protected virtual void Awake()
    {
        skillManager = GetComponentInParent<Player_SkillManager>();
        entity = GetComponentInParent<Entity>();
        lastTimeUsed = lastTimeUsed - cooldown;
    }

    public virtual void TryUseSkill()
    {
        target = FindClosestTarget();

        if (CanUseSkill() == false)
            return;
    }

    public virtual void SetSkillUpgrade(Skill_DataSO skillData)
    {
        UpgradeData upgrade = skillData.upgradeData;
        upgradeType = upgrade.upgradeType;
        cooldown = upgrade.cooldown;
        speed = upgrade.speed;
        checkEnemyRadius = upgrade.distanceToAttack;
        checkDamageRadius = upgrade.attackRadius;
        skillData.SetUpgrade(true);
        speed = skillData.speed;
        duration = skillData.duration;

        //damageScaleData = upgrade.damageScale;

        //player.ui.ingameUI.GetSkillSlot(skillType).SetupSkillSlot(skillData);
        ResetCooldown();
    }

    public virtual bool CanUseSkill()
    {
        if (upgradeType == SkillUpgradeType.None)
        {
            Debug.Log("No Upgrade");
            return false;
        }

        if (target == null)
        {
            Debug.Log("No Target");
            return false;
        }

        if (OnCooldown())
        {
            Debug.Log("On Cooldown");
            return false;
        }

        return true;
    }

    protected bool Unlocked(SkillUpgradeType upgradeToCheck) => upgradeType == upgradeToCheck;
    public SkillUpgradeType GetUpgrade() => upgradeType;
    public SkillType GetSkillType() => skillType;

    protected bool OnCooldown() => Time.time < lastTimeUsed + cooldown;
    public void SetSkillOnCooldown()
    {
        //player.ui.ingameUI.GetSkillSlot(skillType).StartCooldown(cooldown);
        lastTimeUsed = Time.time;
    }
    public void ReduceCooldownBy(float cooldownReduction) => lastTimeUsed = lastTimeUsed + cooldownReduction;
    public void ResetCooldown()
    {
        //player.ui.ingameUI.GetSkillSlot(skillType).ResetCooldown();
        lastTimeUsed = Time.time - cooldown;
    }

    public Transform FindClosestTarget()
    {
        Transform closestTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach (var target in GetEnemyAround(transform, checkEnemyRadius))
        {
            float distance = Vector2.Distance(transform.position, target.transform.position);

            if (distance < closestDistance)
            {
                closestTarget = target.transform;
                closestDistance = distance;
            }
        }

        return closestTarget;
    }

    protected Collider2D[] GetEnemyAround(Transform t, float radius)
    {
        return Physics2D.OverlapCircleAll(t.position, radius, whatIsEnemy);
    }

    protected bool CheckEnemyRadius()
    {
        if (target == null)
            return false;

        return Vector2.Distance(target.position, transform.root.position) < checkEnemyRadius;
    }



    protected virtual void OnDrawGizmos()
    {
        if (targetCheck == null)
            targetCheck = transform;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(targetCheck.position, checkEnemyRadius);
    }


}
