using System;
using UnityEngine;

public class Entity_Health : MonoBehaviour, IDamageable
{
    public event Action OnHealthChanged;
    public Action<int, bool> OnDamagePopup;

    protected Entity entity;

    [SerializeField] private GameObject damagePopupPrefab;
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

        OnDamagePopup += CreateDamagePopup;
    }

    protected virtual void Update()
    {
    }

    protected virtual void OnDisable()
    {
        isDead = false;
    }

    public virtual bool TakeDamage(bool isCrit, float damage, Transform damagedDealer)
    {
        if (currentHealth <= 0 || isDead)
            return false;

        if (CanApplyDamage(entity) == false)
            return false;

        Entity_Stats attackerStats = damagedDealer.GetComponent<Entity_Stats>();

        float armorReduction = attackerStats != null ? attackerStats.GetArmorReduction() : 0f;
        float mitigation = entity.entityStats != null ? entity.entityStats.GetArmorMitigation(armorReduction) : 0f;

        int physicalDamageTaken = Mathf.RoundToInt(damage * (1 - mitigation));
        int finalDamage = physicalDamageTaken;

        if (finalDamage <= 0)
            return false;

        if (damagePopupPrefab != null)
            OnDamagePopup?.Invoke(finalDamage, isCrit);

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
        entity.entitySFX?.PlayDeath();
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

    private bool CanApplyDamage(Entity target)
    {
        float evasion = target.entityStats.GetEvasion();

        if (evasion > UnityEngine.Random.value)
        {
            Debug.Log("Evasion");
            return false;
        }

        return true;
    }

    public void CreateDamagePopup(int damage, bool isCrit)
    {
        Color popupColor = isCrit ? GameColors.DamageCrit : GameColors.DamageNormal;

        GameObject damagePopup = ObjectPool.instance.Spawn(
            damagePopupPrefab.name,
            transform.position,
            Quaternion.identity
        );

        DamagePopup popup = damagePopup.GetComponent<DamagePopup>();
        popup.Setup(damage.ToString(), popupColor);

        ObjectPool.instance.Despawn(damagePopup, 1f);
    }

    public int GetCurrentHealth() => currentHealth;
    protected bool IsDeaded() => currentHealth <= 0;

    public void OnHealthChangedInvoke() => OnHealthChanged?.Invoke();
}
