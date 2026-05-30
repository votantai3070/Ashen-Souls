using System;
using TMPro;
using UnityEngine;

public class UI_Upgrades : MonoBehaviour
{
    public event Action OnUpgradePoints;

    public UI_UpgradeSlot[] upgradeSlots;
    [SerializeField] private GameObject soulsInfo;

    private void Awake()
    {
        upgradeSlots = GetComponentsInChildren<UI_UpgradeSlot>(true);
    }

    private void OnEnable()
    {
        UpdateSoulInfo();
        OnUpgradePoints += UpdateSoulInfo;
    }

    private void OnDisable()
    {
        OnUpgradePoints -= UpdateSoulInfo;
    }

    public void RefundedAllPoints()
    {
        foreach (var slot in upgradeSlots)
        {
            slot.RefundedPoints();
        }

        OnUpgradePointsInvoke();
    }

    private void UpdateSoulInfo()
    {
        soulsInfo.GetComponentInChildren<TextMeshProUGUI>().text = GameManager.instance.TotalSouls.ToString();
    }

    public void OnUpgradePointsInvoke()
    {
        OnUpgradePoints?.Invoke();
    }
}
