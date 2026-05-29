using System.Collections.Generic;
using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    public Entity entity { get; private set; }

    [SerializeField] private HashSet<IDamageable> hitThisAttack = new();

    [SerializeField] protected Transform attackArea;
    [SerializeField] protected LayerMask enemyLayer;
    [SerializeField] protected Collider2D[] showTargetEnemies;

    [Header("PerformAttack Settings")]
    protected float attackRadius = 1f;
    private float lastAttackTime = -999f;
    [SerializeField] private float attackCooldownGuard = 0.1f; // 100ms

    [Space]
    [SerializeField] private float defaultImpactDuration = .1f;

    private bool canHit;
    private bool becomeInvulnerable;
    private bool attackWindow;

    protected virtual void Awake()
    {
        entity = GetComponent<Entity>();
    }

    private void Update()
    {
        if (attackArea != null)
            showTargetEnemies = FindAttackTarget(attackArea);
    }

    public void ResetHitList() => hitThisAttack.Clear();

    public virtual void PerformAttack(Entity dealer)
    {
        if (CanAttack() == false)
            return; // Guard against attacking too frequently

        if (attackWindow == false)
            return;

        lastAttackTime = Time.time;
        hitThisAttack.Clear();

        foreach (Collider2D enemy in FindAttackTarget(attackArea))
        {
            if (enemy.TryGetComponent(out IDamageable damageable))
            {
                if (hitThisAttack.Contains(damageable)) continue;
                hitThisAttack.Add(damageable);

                if (CanApplyDamage(enemy.GetComponent<Entity>()) == false)
                    return;

                float dealerDamage = entity.entityStats.GetPhysicalDamage(out bool isCriticalHit);
                canHit = damageable.TakeDamage(isCriticalHit, dealerDamage, dealer.transform);

                if (canHit)
                {
                    Entity_VFX targetVfx = enemy.GetComponent<Entity_VFX>();

                    if (targetVfx != null && enemy.gameObject.activeInHierarchy)
                        targetVfx.DamageVfx(defaultImpactDuration);

                    if (entity != null && entity.entityVFX != null)
                        entity.entityVFX.GetImapctVfx(enemy.transform, isCriticalHit);

                    if (entity != null && entity.entitySFX != null)
                        entity.entitySFX.PlayAttack();
                }
                else
                {
                    if (entity != null && entity.entitySFX != null)
                        entity.entitySFX.PlayAttackMiss();
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

        if (target.entityCombat.becomeInvulnerable)
        {
            Debug.Log("Become Invulnerable");
            return false;
        }

        return true;
    }

    public virtual Collider2D[] FindAttackTarget(Transform attackArea)
    {
        this.attackArea = attackArea;
        return Physics2D.OverlapCircleAll(attackArea.position, attackRadius, enemyLayer);
    }

    public void SetAttackWindow(bool active) => attackWindow = active;

    public void SetInvulnerable(bool invulnerable) => becomeInvulnerable = invulnerable;

    public bool CanAttack()
    {
        if (Time.time < lastAttackTime + attackCooldownGuard)
            return false;

        return true;
    }
}
