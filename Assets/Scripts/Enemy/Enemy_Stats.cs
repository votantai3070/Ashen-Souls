using UnityEngine;

public class Enemy_Stats : Entity_Stats
{
    private Enemy enemy;

    [Header("Enemy Exp Drop Config")]
    [SerializeField] private float baseExpDrop = 20f;     // EXP base
    [SerializeField] private float expScalingPerLevel = 0.2f; // +20% exp per level player

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    //public override float GetMaxHealth()
    //{
    //    Player player = enemy.GetPlayer();
    //    int playerLevel = player != null ? player.stats.GetLevel() : 1;
    //    float baseMaxHealth = resource.maxHealth.GetValue();

    //    float healthPerLevelPercent = 0.15f;
    //    int levelOffset = Mathf.Max(0, playerLevel - 1);

    //    return baseMaxHealth * (1f + levelOffset * healthPerLevelPercent);
    //}

    public float GetExpDrop()
    {
        Player player = enemy.GetPlayer();
        int playerLevel = player != null ? player.stats.GetLevel() : 1;

        // EXP up to level player: baseExp * (1 + 0.2 * level)
        return Mathf.Floor(baseExpDrop * (1f + expScalingPerLevel * (playerLevel - 1)));
    }
}
