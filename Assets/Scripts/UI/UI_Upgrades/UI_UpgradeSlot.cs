using TMPro;
using UnityEngine;

public class UI_UpgradeSlot : MonoBehaviour
{
    public StatType statType;
    public TextMeshProUGUI upgradeName;
    public UI_UpgradePoint[] upgradePoints;

    private void Awake()
    {
        upgradePoints = GetComponentsInChildren<UI_UpgradePoint>(true);
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

    public void SetRefundedPoints()
    {
        foreach (var point in upgradePoints)
        {
            point.Lock();
        }
    }

    private string GetStatNameByType(StatType type)
    {
        switch (type)
        {
            case StatType.MaxHealth: return "Max Health";
            case StatType.RegenHealth: return "Health Regeneration";
            case StatType.Armor: return "Armor";
            case StatType.Evasion: return "Evasion";

            case StatType.Strength: return "Strength";
            case StatType.Agility: return "Agility";
            case StatType.Intelligence: return "Intelligence";
            case StatType.Vitality: return "Vitality";

            case StatType.AttackSpeed: return "PerformAttack Speed";
            case StatType.Speed: return "Speed";
            case StatType.Damage: return "Damage";
            case StatType.CriticalChance: return "Critical Chance";
            case StatType.CriticalDamage: return "Critical Damage";
            case StatType.ArmorReduction: return "Armor Reduction";

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
}
