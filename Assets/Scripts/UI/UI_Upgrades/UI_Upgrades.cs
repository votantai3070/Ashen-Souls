using System;
using TMPro;
using UnityEngine;

public class UI_Upgrades : MonoBehaviour, ISaveable
{
    public event Action OnUpgradePoints;

    public UI_UpgradeSlot[] upgradeSlots;
    [SerializeField] private GameObject soulsInfo;
    public int totalSouls;

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
        soulsInfo.GetComponentInChildren<TextMeshProUGUI>().text = totalSouls.ToString();
    }

    public void AddSouls(int souls)
    {
        totalSouls += souls;
    }

    public void MinusSouls(int souls)
    {
        totalSouls -= souls;
    }

    public void OnUpgradePointsInvoke()
    {
        OnUpgradePoints?.Invoke();
    }


    public void LoadData(GameData data)
    {
        totalSouls = data.souls;
    }

    public void SaveData(ref GameData data)
    {
        data.souls = totalSouls;
    }
}
