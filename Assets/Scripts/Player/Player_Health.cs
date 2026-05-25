using UnityEngine;

public class Player_Health : Entity_Health
{
    private Player player;
    public float DurationDied { get; set; } = 1f;

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<Player>();
    }

    public float GetHealthPercent()
    {
        return currentHealth / player.stats.GetMaxHealth();
    }

    public float IncreaseHealth(int amount, StatType type)
    {
        if (type == StatType.Vitality)
            currentHealth += amount * 5;
        else if (type == StatType.MaxHealth)
            currentHealth += amount;

        if (currentHealth > player.stats.GetMaxHealth())
            currentHealth = (int)player.stats.GetMaxHealth();
        OnHealthChangedInvoke();
        return currentHealth;
    }

    public override bool TakeDamage(bool isCrit, float damage, Transform damagedDealer)
    {
        return base.TakeDamage(isCrit, damage, damagedDealer);
    }

    protected override void UnBloody()
    {
        if (IsDeaded() || isDead)
        {
            player.TryToDieState();
            UI.instance.OpenSummary(DurationDied);
        }
    }
}
