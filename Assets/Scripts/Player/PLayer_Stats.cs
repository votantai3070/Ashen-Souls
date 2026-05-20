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

        if (buff.type == StatType.MaxHealth)
            player.health.InscreaseHealth(Mathf.RoundToInt(buff.value));
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
}