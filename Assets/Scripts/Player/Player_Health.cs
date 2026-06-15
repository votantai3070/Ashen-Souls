using UnityEngine;

public class Player_Health : Entity_Health
{
    private Player player;
    public float DurationDied { get; set; } = 1f;

    [Header("Health Regen")]
    [SerializeField] private float regenDelay = 3f;
    [SerializeField] private float regenHealthBuffer;
    private float lastTimeTookDamage;
    private float lastTimeHealth;

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<Player>();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        RegenHealth();
    }

    public float GetHealthPercent()
    {
        return currentHealth / player.stats.GetMaxHealth();
    }

    public float IncreaseHealthByType(int amount, StatType type)
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
        if (base.TakeDamage(isCrit, damage, damagedDealer))
        {
            lastTimeTookDamage = Time.time;
            return true;
        }
        return false;
    }

    private void RegenHealth()
    {
        float maxHealth = player.stats.GetMaxHealth();

        if (currentHealth >= maxHealth)
        {
            return;
        }

        if (Time.time < lastTimeTookDamage + regenDelay)
            return;

        if (Time.time < lastTimeHealth + regenDelay)
            return;

        regenHealthBuffer = player.stats.GetRegenHealth();

        if (regenHealthBuffer >= 1f)
        {
            int healAmount = Mathf.FloorToInt(regenHealthBuffer);
            currentHealth += healAmount;
            currentHealth = Mathf.Clamp(currentHealth, 0, Mathf.RoundToInt(maxHealth));

            if (damagePopupPrefab != null)
                OnDamagePopup?.Invoke(healAmount.ToString(), false, false, true);
            OnHealthChangedInvoke();
            lastTimeHealth = Time.time;
        }
    }

    protected override void UnBloody()
    {
        if (IsDeaded() || isDead)
        {
            base.UnBloody();
            player.TryToDieState();
            UI.instance.OpenSummary(DurationDied);
        }
    }

}
