using UnityEngine;

public class SkillObject_Base : MonoBehaviour
{
    public StateMachine<SpellState> stateMachine { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public Animator anim { get; private set; }
    public Collider2D col { get; private set; }

    protected Entity entity;

    [Header("Detected Settings")]
    [SerializeField] protected LayerMask whatIsEnemy;
    [SerializeField] protected Transform targetCheck;
    [SerializeField] protected float checkEnemyRadius = 3;
    public float checkDamageRadius = 3;
    [SerializeField] private float defaultImpactDuration = .2f;

    [Header("Attack Settings")]
    private float lastAttackTime = -999f;
    [SerializeField] private float attackCooldownGuard = 0.1f; // 100ms
    //protected DamageScaleData damageScale;
    //protected ElementType currentElement;
    protected Transform lastTarget;
    protected bool targetGoHit;
    protected SkillUpgradeType upgradeType;

    protected float spawnTime;
    protected float duration;

    protected virtual void Awake()
    {
        col = GetComponent<Collider2D>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        stateMachine = new();

        rb.gravityScale = 0;
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        stateMachine.currentState.Update();
    }

    protected void DamageEnemiesInRadius(Transform t, Transform damageDealer)
    {
        if (!CanAttack()) return; // Guard against attacking too frequently

        lastAttackTime = Time.time;

        foreach (var target in GetEnemyAround(t, checkDamageRadius))
        {
            //if (!target.CompareTag(targetStr))
            //    continue;

            IDamageable damageable = target.GetComponent<IDamageable>();

            Debug.Log("Damageable: " + damageable);

            if (damageable == null) continue;

            //AttackData attackData = playerStats.GetAttackData(damageScale);
            //ElementType element = attackData.element;

            //int physicalDamage = (int)attackData.physicalDamage;
            //int elementalDamage = (int)attackData.elementalDamage;

            int damage = (int)entity.entityStats.GetSkillDamage(upgradeType, out bool isCrit);

            targetGoHit = damageable.TakeDamage(isCrit, damage, damageDealer);

            //if (element != ElementType.None)
            //target.GetComponent<Entity_StatusHandler>().ApplyStatusEffect(element, attackData.effectData);

            if (targetGoHit)
            {
                Debug.Log("Gay dmg");
                if (upgradeType == SkillUpgradeType.FireSoul || upgradeType == SkillUpgradeType.FireSoulUpgrade)
                {
                    target.GetComponent<Entity_VFX>().DamageVfx(defaultImpactDuration);
                    Debug.Log("Fire soul gay dmg");
                    SetPhysicsActive(false);
                }

                target.GetComponent<Entity_VFX>().DamageVfx(defaultImpactDuration);
                entity?.entityVFX?.GetImapctVfx(target.transform, isCrit);
                //lastTarget = target.transform;
                //target.GetComponent<Entity>().ElementalVfx(defaultDuration, element);
            }

            //currentElement = element;
        }
    }

    protected Collider2D[] GetEnemyAround(Transform t, float radius)
    {
        return Physics2D.OverlapCircleAll(t.position, radius, whatIsEnemy);
    }

    public bool CanAttack()
    {
        return Time.time >= lastAttackTime + attackCooldownGuard;
    }

    public void SetPhysicsActive(bool active)
    {
        col.enabled = active;
        rb.simulated = active;
    }

    protected virtual void CheckDuration()
    {

    }

    protected void SetSpeedAnim(float speed) => anim.SetFloat("AttackSpeed", speed);

    protected virtual void OnDrawGizmos()
    {
        if (targetCheck == null)
            targetCheck = transform;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(targetCheck.position, checkDamageRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(targetCheck.position, checkEnemyRadius);
    }
}
