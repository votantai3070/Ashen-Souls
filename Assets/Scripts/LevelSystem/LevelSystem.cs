using System;
using UnityEngine;

[Serializable]
public class LevelSystem
{
    public event Action<int> OnLevelUp;       // level mới
    public event Action<float, float> OnExpChanged; // currentExp, maxExp

    [Header("Config")]
    [SerializeField] private int maxLevel = 99;
    [SerializeField] private float baseExp = 100f;
    [SerializeField] private float expScalingFactor = 1.5f;

    private int currentLevel = 1;
    private float currentExp = 0f;

    // Getter
    public int CurrentLevel() => currentLevel;
    public float CurrentExp() => currentExp;

    public float CurrentMaxExp() => MaxExpForLevel(currentLevel);
    public float ExpProgress() => currentExp / CurrentMaxExp(); // 0 → 1 for Slider

    public float MaxExpForLevel(int level) => Mathf.Floor(baseExp * Mathf.Pow(level, expScalingFactor));

    public void AddExp(float amount)
    {
        if (currentLevel >= maxLevel) return;

        currentExp += amount;

        // Handle get exp fast
        while (currentExp >= CurrentMaxExp() && currentLevel < maxLevel)
        {
            currentExp -= CurrentMaxExp();
            currentLevel++;
            OnLevelUp?.Invoke(currentLevel);
        }

        // Max level -> max exp
        if (currentLevel >= maxLevel)
            currentExp = CurrentMaxExp();

        OnExpChanged?.Invoke(currentExp, CurrentMaxExp());
    }
}
