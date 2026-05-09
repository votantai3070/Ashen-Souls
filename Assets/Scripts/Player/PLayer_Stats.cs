using System.Collections.Generic;
using UnityEngine;

public class Player_Stats : Entity_Stats
{
    private List<string> activeBuff = new List<string>();

    private Player player;
    [SerializeField] private LevelSystem levelSystem = new LevelSystem();

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

    public void ApplyBuff(BuffEffectData[] buffs, string source, bool isPercent)
    {
        if (activeBuff.Contains(source))
        {
            foreach (var buff in buffs)
                GetStatByType(buff.type).RemoveModifier(source);
        }
        else
        {
            activeBuff.Add(source);
        }

        foreach (var buff in buffs)
            GetStatByType(buff.type).AddModifier(buff.value, source, isPercent);
    }

    public void GainExp(float amount) => levelSystem.AddExp(amount);
    public int GetLevel() => levelSystem.CurrentLevel();

    private void HandleLevelUp(int newLevel)
    {
        player?.ui?.OpenSkillBoard();
        player.ui?.ingameUI?.ShowLevelUpEffect(newLevel);
    }

    private void HandleExpChanged(float current, float max)
    {
        player.ui?.ingameUI?.UpdateExpBar(current, max);
        player.ui?.ingameUI?.UpdateLevelText(levelSystem.CurrentLevel());
    }
}