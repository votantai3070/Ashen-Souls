using UnityEngine;

public class Enemy_Health : Entity_Health
{
    private Enemy enemy;
    private DropSystem dropSystem;
    private bool rewardGiven;

    protected override void Awake()
    {
        base.Awake();

        enemy = GetComponent<Enemy>();
        dropSystem = GetComponent<DropSystem>();
    }

    public override bool TakeDamage(bool isCrit, float damage, Transform damagedDealer)
    {
        if (currentHealth <= 0 || isDead)
            return false;

        bool result = base.TakeDamage(isCrit, damage, damagedDealer);

        if (!isDead && currentHealth > 0)
            enemy.TryToIdleState();

        return result;
    }

    protected override void UnBloody()
    {
        if (IsDeaded() && !rewardGiven)
        {
            rewardGiven = true;
            enemy.GetPlayer().stats.GainExp(enemy.stats.GetExpDrop());
            dropSystem.SpawnDrop();

            enemy.player.TryGetComponent<ITotalSummary>(out var totalSummary);
            totalSummary?.AddEnemiesKilled(1);
        }
    }

    protected override void KnockBack(Transform damagedDealer, float damage)
    {
        if (isDead || IsDeaded())
            return;

        base.KnockBack(damagedDealer, damage);
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        currentHealth = (int)enemy.stats.GetMaxHealth();
        rewardGiven = false;
    }
}