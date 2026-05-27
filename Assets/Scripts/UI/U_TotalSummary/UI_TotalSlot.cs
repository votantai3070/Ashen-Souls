using TMPro;
using UnityEngine;

public class UI_TotalSlot : MonoBehaviour
{
    public TextMeshProUGUI totalName;
    public TextMeshProUGUI totalValue;
    public TotalSummaryType totalType;

    private void OnValidate()
    {
        gameObject.name = $"Total Slot - {GetFormattedTotalType()}";
    }

    private void Start()
    {
        totalName.text = GetFormattedTotalType();
    }

    public void SetTotalValue(int value)
    {
        totalValue.text = value.ToString();
    }

    private string GetFormattedTotalType()
    {
        return totalType switch
        {
            TotalSummaryType.ThreatLevel => "Threat Level",
            TotalSummaryType.DamageDealt => "Damage Dealt",
            TotalSummaryType.Time => "Time",
            TotalSummaryType.SoulsGained => "Souls Gained",
            TotalSummaryType.EnemiesKilled => "Enemies Killed",
            TotalSummaryType.LevelReached => "Level Reached",
            TotalSummaryType.ExperienceGained => "Experience Gained",
            _ => totalType.ToString()
        };
    }
}
