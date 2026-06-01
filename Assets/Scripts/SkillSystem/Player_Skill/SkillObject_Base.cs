using UnityEngine;

public class SkillObject_Base : MonoBehaviour
{
    public StateMachine<SpellState> stateMachine { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public Animator anim { get; private set; }
    public Collider2D col { get; private set; }

    protected Entity entity;
    protected SkillUpgradeType upgradeType;

    [Header("Detected Settings")]
    public float checkDamageRadius = 3;
    [SerializeField] protected LayerMask whatIsEnemy;
    [SerializeField] protected Transform targetCheck;
    [SerializeField] protected float checkEnemyRadius = 3;
    [SerializeField] private float defaultImpactDuration = .2f;

    [Header("Attack Settings")]
    private float lastAttackTime = -999f;
    protected float attackCooldownGuard = 0.1f; // 100ms
    protected Transform lastTarget;
    protected bool targetGoHit;

    public float speed { get; protected set; }
    public float damage { get; protected set; }
    public float size { get; protected set; } = 1f;

    //protected ElementType currentElement;
    //protected DamageScaleData damageScale;

    protected float spawnTime;
    protected float duration;

    private bool canHit;
    //private bool becomeInvulnerable;
    private bool attackWindow;

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
        stateMachine.currentState?.Update();
    }

    protected void DamageEnemiesInRadius(Transform t, Transform damageDealer)
    {
        if (!CanAttack()) return; // Guard against attacking too frequently

        Debug.Log("DamageEnemiesInRadius called");

        lastAttackTime = Time.time;

        foreach (var enemy in GetEnemyAround(t, checkDamageRadius))
        {
            if (enemy.TryGetComponent(out IDamageable damageable))
            {
                float dealerDamage = entity.entityStats.GetSkillDamage(damage, out bool isCriticalHit);
                canHit = damageable.TakeDamage(isCriticalHit, dealerDamage, damageDealer.transform);

                if (canHit)
                {
                    if (upgradeType == SkillUpgradeType.FireSoul || upgradeType == SkillUpgradeType.FireSoulUpgrade)
                    {
                        enemy.GetComponent<Entity_VFX>().DamageVfx(defaultImpactDuration);
                        entity?.entityVFX?.GetImapctVfx(enemy.transform, isCriticalHit);
                        SetPhysicsActive(false);
                        return;
                    }

                    Entity_VFX targetVfx = enemy.GetComponent<Entity_VFX>();

                    if (targetVfx != null && enemy.gameObject.activeInHierarchy)
                        targetVfx.DamageVfx(defaultImpactDuration);

                    if (entity != null && entity.entityVFX != null)
                        entity.entityVFX.GetImapctVfx(enemy.transform, isCriticalHit);
                }
            }
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
