using TMPro;
using UnityEngine;

public class UI_Upgrades : MonoBehaviour
{
    public UI_UpgradeSlot[] upgradeSlots;
    [SerializeField] private GameObject soulsInfo;

    private void Awake()
    {
        upgradeSlots = GetComponentsInChildren<UI_UpgradeSlot>(true);
    }

    private void OnEnable()
    {
        UpdateSoulInfo();
    }

    public void RefundedAllPoints()
    {
        foreach (var slot in upgradeSlots)
        {
            slot.RefundedPoints();
        }
    }

    private void UpdateSoulInfo()
    {
        soulsInfo.GetComponentInChildren<TextMeshProUGUI>().text = GameManager.instance.TotalSouls.ToString();
    }
}
