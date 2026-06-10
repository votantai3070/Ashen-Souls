using System.Collections.Generic;
using UnityEngine;

public class Player_Stats : Entity_Stats, ISaveable
{
    private List<string> activeBuff = new List<string>();

    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Start()
    {
        LevelManager.instance.levelSystem.OnLevelUp += HandleLevelUp;
        LevelManager.instance.levelSystem.OnExpChanged += HandleExpChanged;
        HandleExpChanged(LevelManager.instance.levelSystem.CurrentExp(), LevelManager.instance.levelSystem.CurrentMaxExp());
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

    private void HandleLevelUp(int newLevel)
    {
        UI.instance?.OpenSkillBoard();
        UI.instance?.ingameUI?.ShowLevelUpEffect(newLevel);
    }

    private void HandleExpChanged(float current, float max)
    {
        UI.instance?.ingameUI?.UpdateExpBar(current, max);
        UI.instance?.ingameUI?.UpdateLevelText(LevelManager.instance.levelSystem.CurrentLevel());
    }

    public void GainExp(float amount) => LevelManager.instance.levelSystem.AddExp(amount);
    public int GetLevel() => LevelManager.instance.levelSystem.CurrentLevel();

    private float GetBuffValue(StatType type)
    {
        switch (type)
        {
            case StatType.Strength: return 2;
            case StatType.Agility: return 2;
            case StatType.Vitality: return 5;
            case StatType.Speed: return .2f;
            case StatType.CriticalChance: return 4f;
            case StatType.CriticalDamage: return 10f;
            case StatType.ArmorReduction: return 5f;
            case StatType.Damage: return 3f;
            case StatType.MaxHealth: return 20f;
            case StatType.RegenHealth: return .1f;
            case StatType.Armor: return 5f;
            case StatType.Evasion: return 5f;

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
        // Load selected character
        if (!string.IsNullOrEmpty(data.selectedCharacterId))
        {
            PlayerDataSO selectedData = player.characterList.GetCharacterData(data.selectedCharacterId);
            if (selectedData != null)
            {
                DefaultStatSetup(selectedData.defaultCharacterStat);
                ApplyDefaultStatSetup();
            }
            else
            {
                Debug.LogWarning($"PlayerDataSO not found for id: {data.selectedCharacterId}");
            }
        }

        if (data.upgradePoints == null || data.upgradePoints.Count == 0)
            return;

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

    }
}