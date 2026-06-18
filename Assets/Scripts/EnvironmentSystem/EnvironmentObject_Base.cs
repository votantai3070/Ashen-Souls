using UnityEngine;

public class EnvironmentObject_Base : MonoBehaviour
{
    [SerializeField] private float damage;
    private float lastAttackTime;

    [SerializeField] private float attackCooldownGuard = 0.1f; // 100ms

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!CanAttack())
            return;

        if (!collision.TryGetComponent(out IDamageable damageable))
            return;

        damageable.TakeDamage(false, damage, transform);
        lastAttackTime = Time.time;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!CanAttack())
            return;

        if (!collision.TryGetComponent(out IDamageable damageable))
            return;

        damageable.TakeDamage(false, damage, transform);
        lastAttackTime = Time.time;
    }

    public bool CanAttack()
    {
        return Time.time >= lastAttackTime + attackCooldownGuard;
    }
}
