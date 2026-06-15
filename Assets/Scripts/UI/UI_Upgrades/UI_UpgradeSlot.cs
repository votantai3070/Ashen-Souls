using TMPro;
using UnityEngine;

public class UI_UpgradeSlot : MonoBehaviour, ISaveable
{
    private UI ui;
    public StatType statType;

    public TextMeshProUGUI upgradeName;
    private UI_UpgradePointSlot[] upgradePoints;

    private void Awake()
    {
        ui = GetComponentInParent<UI>();
        upgradePoints = GetComponentsInChildren<UI_UpgradePointSlot>(true);
    }

    private void Start()
    {
        upgradeName.text = GetStatNameByType(statType);
    }

    private void OnValidate()
    {
        gameObject.name = statType.ToString() + " - Upgrade Slot";
        upgradeName.text = GetStatNameByType(statType);
    }

    public void RefundedPoints()
    {
        foreach (var point in upgradePoints)
        {
            int soulRefunded = point.IsUnlocked() ? point.GetPointCost() : 0;
            ui.upgradesUI.AddSouls(soulRefunded);
            point.Lock();
        }
    }

    private string GetStatNameByType(StatType type)
    {
        switch (type)
        {
            case StatType.MaxHealth: return "Max Health";
            case StatType.RegenHealth: return "Regen Health";
            case StatType.Armor: return "Armor";
            case StatType.Evasion: return "Evasion";

            case StatType.Strength: return "Strength";
            case StatType.Agility: return "Agility";
            case StatType.Intelligence: return "Intelligence";
            case StatType.Vitality: return "Vitality";

            case StatType.AttackSpeed: return "PerformAttack Speed";
            case StatType.Speed: return "Speed";
            case StatType.Damage: return "Damage";
            case StatType.CriticalChance: return "Cr.Chance";
            case StatType.CriticalDamage: return "Cr.Damage";
            case StatType.ArmorReduction: return "Armor Reduce";

            //case StatType.FireDamage: return "Fire Damage";
            //case StatType.IceDamage: return "Ice Damage";
            //case StatType.LightningDamage: return "Lightning Damage";
            //case StatType.ElementalDamage: return "Elemental Damage";

            //case StatType.FireResistance: return "Fire Resistance";
            //case StatType.IceResistance: return "Ice Resistance";
            //case StatType.LightningResistance: return "Lightning Resistance";
            default: return "Unknow Stat";
        }
    }

    public UI_UpgradePointSlot[] GetUpgradePoints() => upgradePoints;

    public void LoadData(GameData data)
    {
        upgradePoints = GetComponentsInChildren<UI_UpgradePointSlot>(true);

        int unlockedCount = 0;
        if (data.upgradePoints.ContainsKey(statType))
            unlockedCount = data.upgradePoints[statType];

        for (int i = 0; i < upgradePoints.Length; i++)
        {
            if (i < unlockedCount)
                upgradePoints[i].SetUnlocked(true);
            else
                upgradePoints[i].SetUnlocked(false);
        }
    }

    public void SaveData(ref GameData data)
    {
        upgradePoints = GetComponentsInChildren<UI_UpgradePointSlot>(true);

        int unlockedCount = 0;

        foreach (var point in upgradePoints)
        {
            if (point.IsUnlocked())
                unlockedCount++;
        }

        if (unlockedCount > 0)
            data.upgradePoints[statType] = unlockedCount;
        else
            data.upgradePoints.Remove(statType);
    }
}