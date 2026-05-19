using UnityEngine;

public class Player_Health : Entity_Health
{
    private Player player;

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<Player>();
    }

    public float GetHealthPercent()
    {
        return currentHealth / player.stats.GetMaxHealth();
    }

    public float InscreaseHealth(int amount)
    {
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
}
