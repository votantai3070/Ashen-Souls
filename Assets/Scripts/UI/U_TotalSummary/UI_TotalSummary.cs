using UnityEngine;

public class UI_TotalSummary : MonoBehaviour
{
    public UI_TotalSlot[] totalSlots;

    private void Awake()
    {
        totalSlots = GetComponentsInChildren<UI_TotalSlot>(true);
    }

    public void UpdateTotalSummary()
    {
        foreach (var slot in totalSlots)
        {
            int value = 0;
            switch (slot.totalType)
            {
                case TotalSummaryType.DamageDealt:
                    value = (int)GameManager.instance.TotalDamageDealt;
                    break;
                case TotalSummaryType.Time:
                    value = FormatTime(SpawnSystem.instance.GetElapsedTime());
                    break;
                case TotalSummaryType.SoulsGained:
                    value = GameManager.instance.SoulsGained;
                    break;
                case TotalSummaryType.EnemiesKilled:
                    value = (int)GameManager.instance.TotalEnemiesKilled;
                    break;
                case TotalSummaryType.LevelReached:
                    value = LevelManager.instance.levelSystem.CurrentLevel();
                    break;
                case TotalSummaryType.ExperienceGained:
                    value = (int)LevelManager.instance.levelSystem.CurrentExp();
                    break;
            }
            slot.SetTotalValue(value);
        }
    }

    private int FormatTime(int totalSeconds)
    {
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;
        return minutes * 100 + seconds; // Format as MMSS
    }
}
