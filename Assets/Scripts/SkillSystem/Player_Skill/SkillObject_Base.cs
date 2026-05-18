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

    [Header("PerformAttack Settings")]
    private float lastAttackTime = -999f;
    protected float attackCooldownGuard = 0.1f; // 100ms
    //protected DamageScaleData damageScale;
    //protected ElementType currentElement;
    protected Transform lastTarget;
    protected bool targetGoHit;
    protected SkillUpgradeType upgradeType;

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

        lastAttackTime = Time.time;

        foreach (var enemy in GetEnemyAround(t, checkDamageRadius))
        {
            if (enemy.TryGetComponent(out IDamageable damageable))
            {
                if (CanApplyDamage(enemy.GetComponent<Entity>()) == false)
                    return;

                float dealerDamage = entity.entityStats.GetPhysicalDamage(out bool isCriticalHit);
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

    private bool CanApplyDamage(Entity target)
    {
        float evasion = target.entityStats.GetEvasion();

        if (evasion > Random.value)
        {
            Debug.Log("Evasion");
            return false;
        }

        //if (target.entityCombat.becomeInvulnerable)
        //{
        //    Debug.Log("Become Invulnerable");
        //    return false;
        //}

        return true;
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
