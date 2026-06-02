using System.Text;
using TMPro;
using UnityEngine;

public class UI_StatTooltip : UI_Tooltip
{
    [SerializeField] private TextMeshProUGUI statName;
    [SerializeField] private TextMeshProUGUI description;

    public override void ShowTooltip(bool show, RectTransform target)
    {
        base.ShowTooltip(show, target);
    }

    public void ShowTooltip(bool show, RectTransform target, UI_StatSlot statSlotUI = null)
    {
        base.ShowTooltip(show, target);

        if (show == false)
            return;

        StatType statType = statSlotUI.GetStatType();

        statName.text = statSlotUI.GetStatNameByType(statType);
        description.text = GetDescriptionInfo(statType);
    }

    private string GetDescriptionInfo(StatType statType)
    {
        StringBuilder sb = new StringBuilder();

        switch (statType)
        {
            case StatType.Strength:
                sb.AppendLine("+ 0.5 DAMAGE per STR");
                sb.AppendLine("+ 0.5% CRIT DAMAGE per STR");
                break;
            case StatType.Agility:
                sb.AppendLine("+ 0.5 SPEED per AGI");
                sb.AppendLine("+ 0.3% CRIT CHANCE per AGI");
                break;
            case StatType.Vitality:
                sb.AppendLine("+ 5 MAX HEALTH per VIT");
                sb.AppendLine("+ 0.2 ARMOR per VIT");
                break;
        }

        return sb.ToString();
    }
}

