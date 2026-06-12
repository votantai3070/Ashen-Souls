using TMPro;
using UnityEngine;

public class Enemy_Health : Entity_Health
{
    private Enemy enemy;
    private DropSystem dropSystem;

    [Header("Health Bar")]
    [SerializeField] private UI_Bar healthBar;
    [SerializeField] private TextMeshProUGUI healthText;
    [Space]
    private bool rewardGiven;

    protected override void Awake()
    {
        base.Awake();

        enemy = GetComponent<Enemy>();
        dropSystem = GetComponent<DropSystem>();
    }

    protected override void Start()
    {
        base.Start();
        OnHealthChanged += UpdateHealthBarUI;
        UpdateHealthBarUI();
    }

    private void OnDestroy()
    {
        OnHealthChanged -= UpdateHealthBarUI;
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

    private void UpdateHealthBarUI()
    {
        if (healthBar != null)
        {
            float percent = currentHealth / enemy.stats.GetMaxHealth();
            healthBar.SetFill(percent);
        }

        if (healthText != null)
        {
            healthText.text = currentHealth.ToString();
        }
    }


    protected override void UnBloody()
    {
        if (IsDeaded() && !rewardGiven)
        {
            base.UnBloody();

            enemy.player.TryGetComponent<ITotalSummary>(out var totalSummary);

            if (enemy.enemyType == EnemyType.Boss)
            {
                SpawnSystem.instance.OnBossDefeated();
            }

            dropSystem.SpawnDrop();
            rewardGiven = true;
            enemy.GetPlayer().stats.GainExp(enemy.stats.GetExpDrop());

            totalSummary?.AddEnemiesKilled();
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