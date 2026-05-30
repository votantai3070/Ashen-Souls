using TMPro;
using UnityEngine;

public class UI_UpgradeSlot : MonoBehaviour, ISaveable
{
    public StatType statType;

    public TextMeshProUGUI upgradeName;
    private UI_UpgradePointSlot[] upgradePoints;

    private void Awake()
    {
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
            GameManager.instance.AddSouls(soulRefunded);
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

    public void LoadData(GameData data)
    {
        upgradePoints = GetComponentsInChildren<UI_UpgradePointSlot>(true);
        foreach (var point in upgradePoints)
        {
            if (data.upgradePoints.ContainsKey(statType) && data.upgradePoints[statType] > 0)
            {
                point.Unlock();
                data.upgradePoints[statType] -= 1;
            }
            else
            {
                point.Lock();
            }
        }
    }

    public void SaveData(ref GameData data)
    {
        foreach (var point in upgradePoints)
        {
            if (point.IsUnlocked())
            {
                if (data.upgradePoints.ContainsKey(statType))
                    data.upgradePoints[statType] += 1;
                else
                    data.upgradePoints.Add(statType, 1);
            }
        }
    }
}