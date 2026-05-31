using System.Collections.Generic;
using UnityEngine;

public class Player_Stats : Entity_Stats, ISaveable
{
    private List<string> activeBuff = new List<string>();

    private Player player;
    public LevelSystem levelSystem = new LevelSystem();

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Start()
    {
        levelSystem.OnLevelUp += HandleLevelUp;
        levelSystem.OnExpChanged += HandleExpChanged;
        HandleExpChanged(levelSystem.CurrentExp(), levelSystem.CurrentMaxExp());
    }

    public void ApplyBuff(BuffEffectData buff, string source, bool isPercent)
    {
        if (activeBuff.Contains(source))
        {
            GetStatByType(buff.type).UpdateModifier(buff.value, source, isPercent);
        }
        else
        {
            activeBuff.Add(source);
            GetStatByType(buff.type).AddModifier(buff.value, source, isPercent);
        }

        OnStatChangedInvoke();

        player.health.IncreaseHealth(Mathf.RoundToInt(buff.value), buff.type);
    }

    public void GainExp(float amount) => levelSystem.AddExp(amount);
    public int GetLevel() => levelSystem.CurrentLevel();

    private void HandleLevelUp(int newLevel)
    {
        UI.instance?.OpenSkillBoard();
        UI.instance?.ingameUI?.ShowLevelUpEffect(newLevel);
    }

    private void HandleExpChanged(float current, float max)
    {
        UI.instance?.ingameUI?.UpdateExpBar(current, max);
        UI.instance?.ingameUI?.UpdateLevelText(levelSystem.CurrentLevel());
    }

    private float GetBuffValue(StatType type)
    {
        switch (type)
        {
            case StatType.Strength: return 5;
            case StatType.Agility: return 3;
            case StatType.Vitality: return 10;
            case StatType.Speed: return .2f;
            case StatType.CriticalChance: return 4f;
            case StatType.CriticalDamage: return 10f;
            case StatType.ArmorReduction: return 5f;
            case StatType.Damage: return 5f;
            case StatType.MaxHealth: return 100;
            case StatType.RegenHealth: return .1f;
            case StatType.Armor: return 5f;
            case StatType.Evasion: return 20;

            default: return 0f;
        }
    }

    private bool IsPercentageStat(StatType type)
    {
        switch (type)
        {
            case StatType.Speed:
            case StatType.ArmorReduction:
            case StatType.RegenHealth:
                return true;

            default: return false;
        }
    }

    public void LoadData(GameData data)
    {
        if (data.upgradePoints == null && data.upgradePoints.Count == 0)
        {
            return;
        }

        foreach (var point in data.upgradePoints)
        {
            var stat = point.Key;
            var value = point.Value * GetBuffValue(stat);

            string source = $"UpgradePoint_{stat}";
            bool isPercent = IsPercentageStat(stat);

            GetStatByType(stat).AddModifier(value, source, isPercent);
        }
    }

    public void SaveData(ref GameData data)
    {
        //if (data.upgradePoints == null && data.upgradePoints.Count == 0)
        //{
        //    return;
        //}

        //foreach (var point in data.upgradePoints)
        //{
        //    var stat = point.Key;
        //    string source = $"UpgradePoint_{stat}";

        //    GetStatByType(stat).RemoveModifier(source);
        //}
    }
}