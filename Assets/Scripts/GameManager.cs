using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int currentLevel = 1;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }



    public string GetStatNameByType(StatType type)
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

    public bool IsPercentageStat(StatType type)
    {
        switch (type)
        {
            case StatType.CriticalChance:
            case StatType.CriticalDamage:
            //case StatType.FireResistance:
            //case StatType.IceResistance:
            //case StatType.LightningResistance:
            case StatType.ArmorReduction:
            case StatType.AttackSpeed:
            case StatType.Evasion:
                return true;

            default: return false;
        }
    }
}
