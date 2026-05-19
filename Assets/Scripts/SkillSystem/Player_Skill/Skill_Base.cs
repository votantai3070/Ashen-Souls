using UnityEngine;

public class Skill_Base : MonoBehaviour
{
    public Player_SkillManager skillManager { get; private set; }
    public Entity entity { get; private set; }
    public Skill_DataSO skillData { get; private set; }

    [Space]
    public LayerMask whatIsEnemy;
    public Transform target;
    [SerializeField] protected Transform targetCheck;

    [Header("General details")]
    public SkillType skillType;

    public float speed;
    public float checkEnemyRadius;
    public float checkDamageRadius;
    public float attackCooldownGuard;
    [SerializeField] protected float cooldown;
    [SerializeField] protected float duration = 5f;

    [Header("Level Details")]
    public int currentLevel = 1;
    public int maxLevel = 10;

    [Header("Stat Details")]
    public Stat damageSkill;
    public Stat speedSkill;
    public Stat sizeSkill;
    public Stat attackSpeedSkill;

    private float lastTimeUsed;


    protected virtual void Awake()
    {
        skillManager = GetComponentInParent<Player_SkillManager>();
        entity = GetComponentInParent<Entity>();
        lastTimeUsed = lastTimeUsed - cooldown;
    }

    public virtual bool TryUseSkill()
    {
        target = FindClosestTarget();

        if (CanUseSkill() == false)
            return false;

        return true;
    }

    public virtual void SetSkill(Skill_DataSO skillData)
    {
        this.skillData = skillData;
        skillType = skillData.skillType;

        attackSpeedSkill.SetBaseValue(skillData.cooldown);
        speedSkill.SetBaseValue(skillData.speed);
        sizeSkill.SetBaseValue(skillData.size);
        damageSkill.SetBaseValue(skillData.damage);

        cooldown = attackSpeedSkill.GetValue();
        checkEnemyRadius = skillData.distanceToAttack;
        checkDamageRadius = skillData.size;
        duration = skillData.duration;
        attackCooldownGuard = skillData.attackCooldownGuard;

        UI.instance.OnSkillSlotChangeInvoke();

        ResetCooldown();
    }

    public void SetSkillUpgrade(SkillBuff_DataSO skillData)
    {
        GetStat(skillData.skillStatData.type).AddModifier(skillData.skillStatData.value, skillData.displayName, skillData.isPercent);
    }

    public virtual bool CanUseSkill()
    {
        if (OnCooldown())
        {
            Debug.Log("On Cooldown");
            return false;
        }

        if (skillType == SkillType.DeathDash)
            return true;

        if (target == null)
        {
            Debug.Log("No Target");
            return false;
        }


        return true;
    }

    private Stat GetStat(StatType upgradeType)
    {
        return upgradeType switch
        {
            StatType.Damage => damageSkill,
            StatType.Speed => speedSkill,
            StatType.Size => sizeSkill,
            _ => null
        };
    }

    public SkillType GetSkillType() => skillType;

    protected bool OnCooldown() => Time.time < lastTimeUsed + cooldown;
    public void SetSkillOnCooldown()
    {
        UI.instance.ingameUI.skillHolder.StartCooldownSkillSlot(skillType, cooldown);
        lastTimeUsed = Time.time;

        if (attackSpeedSkill.GetValue() != cooldown)
        {
            cooldown = attackSpeedSkill.GetValue();
        }
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

    public float GetCooldown() => cooldown;
    public float GetDuration() => duration;


    protected virtual void OnDrawGizmos()
    {
        if (targetCheck == null)
            targetCheck = transform;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(targetCheck.position, checkEnemyRadius);
    }


}
