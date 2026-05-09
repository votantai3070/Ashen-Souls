using UnityEngine;

public class Enemy_Health : Entity_Health
{
    private Enemy enemy;
    private DropSystem dropSystem;

    protected override void Awake()
    {
        base.Awake();

        enemy = GetComponent<Enemy>();
        dropSystem = GetComponent<DropSystem>();
    }

    protected override void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(true, 999, enemy.player);
        }
    }

    public override bool TakeDamage(bool isCrit, float damage, Transform damagedDealer)
    {
        enemy.TryToIdleState();

        return base.TakeDamage(isCrit, damage, damagedDealer);
    }

    protected override void UnBloody()
    {
        if (currentHealth <= 0)
        {
            enemy.GetPlayer().stats.GainExp(enemy.stats.GetExpDrop());
            dropSystem.SpawnDrop();
            enemy.TryToDieState();
        }
    }


    private void OnDisable()
    {
        currentHealth = (int)enemy.stats.GetMaxHealth();
    }
}
