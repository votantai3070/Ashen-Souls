using UnityEngine;

public class UI_Upgrades : MonoBehaviour
{
    public UI_UpgradeSlot[] upgradeSlots;

    private void Awake()
    {
        upgradeSlots = GetComponentsInChildren<UI_UpgradeSlot>(true);
    }

    public void RefundedAllPoints()
    {
        foreach (var slot in upgradeSlots)
        {
            slot.SetRefundedPoints();
        }
    }
}
