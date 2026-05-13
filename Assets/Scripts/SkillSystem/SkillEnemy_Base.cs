using UnityEngine;
using static SkillEnemy_DataSO;

public class SkillEnemy_Base : MonoBehaviour
{
    private SkillEnemy_DataSO skillEnemyData;
    public Entity entity { get; private set; }
    public Enemy enemy { get; private set; }
    private Vector2 direction;
    [SerializeField] private GameObject energyBallGo;
    public SkillEnemyType skillEnemyType;
    [SerializeField] private LayerMask whatIsEnemy;
    public Transform target;

    private GameObject energyBall;

    [Header("Settings")]
    public float cooldown;
    public float speed;
    public float checkEnemyRadius;
    public float checkDamageRadius;
    public float duration;

    private float lastTimeUsed;

    private void Awake()
    {
        entity = GetComponentInParent<Entity>();
        enemy = entity as Enemy;
    }

    public void SetSkillUpgrade(SkillEnemy_DataSO skillData)
    {
        UpgradeData upgrade = skillData.upgradeData;
        skillEnemyType = upgrade.enemyType;
        whatIsEnemy = skillData.whatIsEnemy;
        energyBallGo = skillData.skillObjectPrefab;
        skillEnemyData = skillData;

        cooldown = upgrade.cooldown;
        speed = upgrade.speed;
        checkEnemyRadius = upgrade.distanceToAttack;
        checkDamageRadius = upgrade.attackRadius;
        duration = skillData.duration;

        lastTimeUsed = Time.time;
    }

    public bool TryUseSkill()
    {
        target = FindClosestTarget();

        if (skillEnemyData == null)
            return false;

        if (target == null)
            return false;

        if (OnCooldown())
            return false;

        if (energyBall != null)
            return false;

        SetOnCooldown();
        CreateEnergyBall();

        return true;
    }

    private void CreateEnergyBall()
    {
        direction = enemy.GetDirectionPlayer();

        energyBall = ObjectPool.instance.Spawn(energyBallGo.name, transform.position, Quaternion.identity);
        energyBall.GetComponent<SkillObjectEnemy_EnergyBall>().SetupEnergyBall(this, duration, direction, whatIsEnemy);
    }

    public void OnSoulBurstExpired()
    {
        energyBall = null;
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
