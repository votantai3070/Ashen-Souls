using System;
using UnityEngine;

public class Entity_Health : MonoBehaviour, IDamageable
{
    public event Action OnHealthChanged;

    protected Entity entity;

    [Space]
    [SerializeField] protected int currentHealth;
    public bool isDead;

    protected virtual void Awake()
    {
        entity = GetComponent<Entity>();
    }

    protected virtual void Start()
    {
        currentHealth = (int)entity.entityStats.GetMaxHealth();
    }

    protected virtual void Update()
    {
    }

    public virtual bool TakeDamage(bool isCrit, float damage, Transform damagedDealer)
    {
        if (currentHealth <= 0 || isDead)
            return false;

        Entity_Stats attackerStats = damagedDealer.GetComponent<Entity_Stats>();

        float armorReduction = attackerStats != null ? attackerStats.GetArmorReduction() : 0f;
        float mitigation = entity.entityStats != null ? entity.entityStats.GetArmorMitigation(armorReduction) : 0f;

        int physicalDamageTaken = Mathf.RoundToInt(damage * (1 - mitigation));
        int finalDamage = physicalDamageTaken;

        if (finalDamage <= 0)
            return false;

        ITotalSummary dealer = damagedDealer.GetComponent<ITotalSummary>();
        if (dealer != null)
            dealer.AddDamageDealt(finalDamage);

        bool willDie = currentHealth - finalDamage <= 0;

        if (!willDie)
            KnockBack(damagedDealer, physicalDamageTaken);

        ReduceHp(finalDamage);
        UnBloody();

        return true;
    }

    public void ReduceHp(int damage)
    {
        currentHealth -= damage;

        if (currentHealth < 0)
            currentHealth = 0;

        OnHealthChangedInvoke();

        if (IsDeaded() && !isDead)
            Die();
    }

    protected virtual void UnBloody()
    {

    }

    protected virtual void KnockBack(Transform damagedDealer, float damage)
    {
        TakeKnockback(damagedDealer, damage);
    }

    protected virtual void TakeKnockback(Transform damagedDealer, float finalDamage)
    {
        //float averangeDamage = finalDamage / entityStats.GetMaxHealth();
        float averangeDamage = finalDamage / 100f;

        entity?.KnockBack(damagedDealer, averangeDamage);
    }

    protected virtual void Die()
    {
        if (isDead)
            return;

        isDead = true;
        entity?.TryToDieState();
    }

    public int GetCurrentHealth() => currentHealth;
    protected bool IsDeaded() => currentHealth <= 0;

    public void OnHealthChangedInvoke() => OnHealthChanged?.Invoke();

    protected virtual void OnDisable()
    {
        isDead = false;
    }
}
