using System;
using UnityEngine;

public class Entity_Health : MonoBehaviour, IDamageable
{
    public event Action OnHealthChanged;

    protected Entity entity;

    [Space]
    [SerializeField] protected int currentHealth;

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
        if (currentHealth <= 0)
            return false;

        Entity_Stats attackerStats = damagedDealer.GetComponent<Entity_Stats>();

        float armorReduction = attackerStats != null ? attackerStats.GetArmorReduction() : 0f;
        float migitation = entity.entityStats != null ? entity.entityStats.GetArmorMitigation(armorReduction) : 0;

        int physicalDamageTaken = Mathf.RoundToInt(damage * (1 - migitation));

        int finalDamage = physicalDamageTaken;

        TakeKnockback(damagedDealer, physicalDamageTaken);
        ReduceHp(finalDamage);

        UnBloody();

        return true;
    }

    public void ReduceHp(int damage)
    {
        currentHealth -= damage;
        OnHealthChanged?.Invoke();

        if (currentHealth < 0)
            currentHealth = 0;
    }

    protected virtual void UnBloody()
    {

    }

    public virtual void Die()
    {
        ObjectPool.instance.Despawn(gameObject);
    }

    protected virtual void TakeKnockback(Transform damagedDealer, float finalDamage)
    {
        //float averangeDamage = finalDamage / entityStats.GetMaxHealth();
        float averangeDamage = finalDamage / 100f;

        entity?.KnockBack(damagedDealer, averangeDamage);
    }

    public int GetCurrentHealth() => currentHealth;
}
