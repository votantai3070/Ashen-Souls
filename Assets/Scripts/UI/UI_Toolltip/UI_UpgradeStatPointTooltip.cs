using TMPro;
using UnityEngine;

public class UI_UpgradeStatPointTooltip : UI_Tooltip
{
    [SerializeField] private TextMeshProUGUI pointCostText;
    public override void ShowTooltip(bool show, RectTransform target)
    {
        base.ShowTooltip(show, target);
    }

    public void ShowTooltip(bool show, RectTransform target, UI_UpgradePointSlot upgradePointSlotUI = null)
    {
        base.ShowTooltip(show, target);

        if (show == false)
            return;

        if (upgradePointSlotUI == null && upgradePointSlotUI.GetPointCost() <= 0)
            return;

        int pointCost = upgradePointSlotUI.GetPointCost();

        pointCostText.text = $"Soul: {pointCost}";
    }
}
