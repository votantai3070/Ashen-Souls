using UnityEngine;
using static SkillEnemy_DataSO;

public class SkillEnemy_Base : MonoBehaviour
{
    private SkillEnemy_DataSO skillEnemyData;
    public Entity entity { get; private set; }
    public Enemy enemy { get; private set; }
    private Vector2 direction;
    [SerializeField] private GameObject skillObjectGo;
    public SkillEnemyType skillEnemyType;
    [SerializeField] private LayerMask whatIsEnemy;
    public Transform target;

    private GameObject skillObject;

    [Header("Settings")]
    public float damage;
    public float cooldown;
    public float speed;
    public float checkEnemyRadius;
    public float checkDamageRadius;
    public float duration;
    public float attackCooldownGuard;

    private float lastTimeUsed;

    private void Awake()
    {
        entity = GetComponentInParent<Entity>();
        enemy = entity as Enemy;
    }

    public virtual void SetSkillUpgrade(SkillEnemy_DataSO skillData)
    {
        SkillEnemyData skill = skillData.upgradeData;
        skillEnemyType = skill.enemyType;
        whatIsEnemy = skillData.whatIsEnemy;
        skillObjectGo = skillData.skillObjectPrefab;
        skillEnemyData = skillData;

        damage = skill.damage;
        cooldown = skill.cooldown;
        speed = skill.speed;

        checkEnemyRadius = skill.distanceToAttack;
        enemy.chaseStopDistance = skill.distanceToAttack;
        enemy.attackDistanceToPlayer = skill.distanceToAttack;
        attackCooldownGuard = skill.attackCooldownGuard;

        checkDamageRadius = skill.attackRadius;
        duration = skillData.duration;

        lastTimeUsed = Time.time;
    }

    public virtual bool TryUseSkill()
    {
        target = FindClosestTarget();

        if (CanUseSkill() == false)
            return false;

        return true;
    }

    public bool CanUseSkill()
    {
        if (skillEnemyData == null) return false;

        if (target == null) return false;

        if (OnCooldown()) return false;

        if (skillObject != null) return false;

        return true;
    }

    protected virtual void CreateSkillObject()
    {
        direction = enemy.GetDirectionPlayer();

        skillObject = ObjectPool.instance.Spawn(skillObjectGo.name, transform.position, Quaternion.identity);
        skillObject.GetComponent<SkillObjectEnemy_EnergyBall>().SetupEnergyBall(this, duration, direction, whatIsEnemy);
    }

    public void OnSoulBurstExpired()
    {
        skillObject = null;
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

    public void SetOnCooldown() => lastTimeUsed = Time.time;
    public bool OnCooldown() => Time.time < lastTimeUsed + cooldown;
}
